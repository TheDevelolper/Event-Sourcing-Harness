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

await builder
    .AddLoggingConfiguration()
    .AddServiceDefaults()
    .AddAuthentication()
    .AddEventStore(eventsDbConnectionString)
    .AddFeatureModules(featureModules);

builder.Services.AddControllers();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
await app.AddFeatureMiddleware(featureModules);
await app.RunAsync();
