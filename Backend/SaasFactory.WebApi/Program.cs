using Serilog;
using Serilog.Debugging;
using Serilog.Formatting.Display;
using Serilog.Sinks.Grafana.Loki;
using SaasFactory.Features.UserSubscriptions;
using SaasFactory.WebApi.Extensions;
using SaasFactory.Shared.Config;
using Ardalis.GuardClauses;
using Mediator.Net;
using Mediator.Net.MicrosoftDependencyInjection;
using SaasFactory.EventSourcing.Marten;
using SaasFactory.Features.Authentication;
using SaasFactory.Modules.Common;
using Scalar.AspNetCore;
using Serilog.Core;
using ILogger = Serilog.ILogger;

try
{
    // Enable Serilog internal diagnostics
    SelfLog.Enable(Console.Error);
    
    var logger = CommonLoggerFactory.CreateLogger("SaasFactory WebApi");

    Log.Logger = logger;

    Log.Information("Bootstrapping WebApi...");

    var builder = WebApplication.CreateBuilder(args);

    Log.Information("Loading configuration...");

    builder.Configuration
        .UseSharedConfiguration()
        .AddJsonFile("appsettings.json", optional: true)
        .Build();

    Log.Information("Configuration loaded.");

    Log.Information("Resolving event store connection string from ENV.");
    var eventsDbConnectionString =
        Environment.GetEnvironmentVariable("EVENTS_DB_CONNECTION")
        ?? throw new InvalidOperationException("Missing Postgres connection string");

    Log.Information("Event store connection string found.");

    var clientSecretEnvVar = builder.Configuration["Authentication:ClientSecretEnvironmentVar"] ?? string.Empty;

    Guard.Against.NullOrWhiteSpace(input: clientSecretEnvVar,
        message: @"CLIENT SECRET ENVIRONMENT VARIABLE NAME IS MISSING FROM CONFIGURATION.
    follow the Authentication Client Secret Setup for reference.
    http://localhost:4400/docs/guides/authentication/authentication-client-secret-setup.html#1-add-the-configuration-setting");

    var clientSecret = Environment.GetEnvironmentVariable(clientSecretEnvVar) ?? string.Empty;
    Guard.Against.NullOrWhiteSpace(input: clientSecret,
        message: @$"CLIENT SECRET ENVIRONMENT VARIABLE IS MISSING.
    Environment Variable Name: {clientSecretEnvVar}
    follow the Authentication Client Secret Setup for reference:
    http://localhost:4400/docs/guides/authentication/authentication-client-secret-setup.html#2-set-the-environment-variable-on-the-host-system");

    Log.Information("Authentication client secret env var key found ✅");

    Log.Information("Configuring services...");

    builder.Logging
        .ClearProviders()
        .AddConsole()
        .SetMinimumLevel(LogLevel.Debug);

    var mediaBuilder = new MediatorBuilder();
    mediaBuilder.RegisterHandlers(
        typeof(Program).Assembly,
        typeof(IUserSubscriptionMarker).Assembly
        ).Build();
    
    builder.Services
        .RegisterMediator(mediaBuilder)
        .AddUserSubscriptionServices()
        .AddAuthentication(clientSecret, logger)
        .AddEventStore(builder, eventsDbConnectionString)
        .AddControllers();

    // Development only services
    if (builder.Environment.IsDevelopment())
    {
        builder.Services.AddOpenApi();
    }

    Log.Information("Services configured.");

    Log.Information("Building middleware pipeline...");
    var app = builder.Build();
    app.UseHttpsRedirection() // IMPORTANT if someone hits http:// first
        .UseCookiePolicy() // <-- required so the policy above is applied
        .UseAuthentication()
        .UseAuthorization();

    // Map Endpoints
    Log.Information("Mapping endpoints...");
    app.MapUserSubscriptionEndpoints(logger);
    app.MapControllers();
    Log.Information("Added endpoints");

    // Development only middleware
    if (app.Environment.IsDevelopment())
    {
        Log.Information("Adding swagger");
        app.MapOpenApi();
        app.MapScalarApiReference();
        Log.Information("Swagger added");
    }

    Log.Information("Middleware configured.");

    // Lifecycle hooks
    app.Lifetime.ApplicationStarted.Register(() =>
    {
        Log.Information("WebApi started. Environment: {Environment}. ContentRoot: {ContentRoot}",
            app.Environment.EnvironmentName, app.Environment.ContentRootPath);
    });
    app.Lifetime.ApplicationStopping.Register(() => { Log.Information("WebApi stopping..."); });
    app.Lifetime.ApplicationStopped.Register(() =>
    {
        Log.Information("WebApi stopped. Flushing logs...");
        Log.CloseAndFlush();
    });

    Log.Information("Running WebApi...");
    await app.RunAsync();
}
catch (Exception ex)
{
    // Log *any* fatal startup errors (e.g., missing config, bad connection, etc.)
    Log.Fatal(ex, "WebApi terminated unexpectedly during startup.");
}
finally
{
    // Ensure all sinks (including Loki) flush
    Log.CloseAndFlush();
}