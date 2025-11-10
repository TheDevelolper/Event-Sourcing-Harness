using Ardalis.GuardClauses;
using Modules.Examples.Bank.Account;
using Modules.Examples.Restaurant.Menu;
using SaasFactory.Features.Authentication;
using SaasFactory.EventSourcing.Marten;
using SaasFactory.Features.UserSubscriptions;
using SaasFactory.Modules.Common;
using SaasFactory.ServiceDefaults;
using SaasFactory.Shared.Config;
using SaasFactory.WebApi.Extensions;
using Serilog;
using Serilog.Debugging;
using Serilog.Formatting.Display;
using Serilog.Sinks.Grafana.Loki;

try
{
    // Enable Serilog internal diagnostics
    SelfLog.Enable(Console.Error);

    var lokiUrl = Environment.GetEnvironmentVariable("LOKI_URL");
    var textFormatter = new MessageTemplateTextFormatter("{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}", null);

    var logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.GrafanaLoki(
            lokiUrl,
            textFormatter: textFormatter, // ✅ render as plain text
            labels: new[]
            {
                new LokiLabel { Key = "app", Value = "saasfactory" },
                new LokiLabel { Key = "env", Value = "dev" }
            })
        .CreateLogger();

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

    /* TODO: This is overkill! 
      I think we could move features into modules/features. As they ARE actually modules.
      The domain modules should sit under modules/domain. Also this registration stuff is just being 
      too clever, maybe instead just register the services and endpoints in the usual way.
    */
    List<IFeatureModule> featureModules =
    [
        new BankAccountModule(),
        new RestaurantMenuModule()
    ];

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
    await builder
        .AddLoggingConfiguration()
        .AddServiceDefaults()
        .AddAuthentication(clientSecret, logger)
        .AddEventStore(eventsDbConnectionString)
        .AddFeatureModules(featureModules);
    Log.Information("Services configured.");

    builder.Services.AddControllers();

    Log.Information("Building middleware pipeline...");
    var app = builder.Build();
    app.UseHttpsRedirection(); // critical if someone hits http:// first
    app.UseCookiePolicy(); // <-- required so the policy above is applied
    app.UseAuthentication();
    app.UseAuthorization();
    
    // Map Endpoints
    Log.Information("Mapping module endpoints...");
    app.MapUserSubscriptionEndpoints(logger);
    app.MapControllers();
    
    await app.AddFeatureMiddleware(featureModules);
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