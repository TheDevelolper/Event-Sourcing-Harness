using Serilog;
using Serilog.Debugging;
using SaasFactory.UserSubscriptions;
using SaasFactory.Shared.Config;
using Ardalis.GuardClauses;
using Mediator.Net;
using Mediator.Net.MicrosoftDependencyInjection;
using SaasFactory.Authentication;
using SaasFactory.Modules.Common;
using Scalar.AspNetCore;
using Modules.Examples.Bank.Account; // Todo standardize module namespaces probably should be SaasFactory.Modules.Examples.Bank.Account
using SaasFactory.EventSourcing.Marten;
using SaasFactory.Logging;

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
    
    // register module
    List<IFeatureModule> coreModules =
    [
        new AuthenticationModule(logger)
    ];
    
    List<IFeatureModule> domainModules =
    [
        new BankAccountModule(logger)
    ];
    
    List<IFeatureModule> modules = [];
    modules.AddRange(coreModules);
    modules.AddRange(domainModules);
    
    // todo: Registration should return a
    // result type and we should handle failure with a log.
    modules.ForEach(async module => await module.RegisterModule(builder));

    builder.Services
        .RegisterMediator(mediaBuilder)
        .AddUserSubscriptionServices()
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
    
     // register module services
     foreach (var module in modules)
     {
         await module.AddModuleMiddleware(app);
     }
     
     app.UseStaticFiles();
     
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
    app.Lifetime.ApplicationStopping.Register(() => Log.Information("WebApi stopping..."));
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