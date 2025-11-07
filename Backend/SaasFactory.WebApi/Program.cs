using Ardalis.GuardClauses;

using Modules.Examples.Bank.Account;
using Modules.Examples.Restaurant.Menu;
using SaasFactory.Features.Authentication;
using SaasFactory.EventSourcing.Marten;
using SaasFactory.Modules.Common;
using SaasFactory.ServiceDefaults;
using SaasFactory.Shared.Config;
using SaasFactory.WebApi.Extensions;

var loggerFactory = LoggerFactory.Create(cfg => cfg.AddConsole());
var logger = loggerFactory.CreateLogger("Startup");

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .UseSharedConfiguration()
    .AddJsonFile("appsettings.json", optional: true)
    .Build();


var eventsDbConnectionString =
    Environment.GetEnvironmentVariable("EVENTS_DB_CONNECTION")
    ?? throw new InvalidOperationException("Missing Postgres connection string");

// TODO: Document as known issue (awaiting docFx Task completion)
// connect and start listening, if this line hangs it's likely connectivity issues.
// It could also be that the featurehub volume was cleared and so the API key is not correct. 
// logger.LogDebug("Connecting to featureHub");
// await featureHubConfig.Init(); // Connect and start listening
// var featureHubCtx = await featureHubConfig.NewContext().Build();
// logger.LogDebug("Connecting to featureHub connected");

List<IFeatureModule> featureModules = [
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

await builder
    .AddLoggingConfiguration()
    .AddServiceDefaults()
    .AddAuthentication(clientSecret)
    .AddEventStore(eventsDbConnectionString)
    .AddFeatureModules(featureModules);

builder.Services.AddControllers();

var app = builder.Build();
app.UseHttpsRedirection();     // critical if someone hits http:// first
app.UseCookiePolicy();         // <-- required so the policy above is applied
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
await app.AddFeatureMiddleware(featureModules);
await app.RunAsync();
