using FeatureHubSDK;
using Modules.Examples.Bank.Account;
using Modules.Examples.Restaurant.Menu;
using SaasFactory.EventSourcing.Marten;
using SaasFactory.Messaging.MasTransit;
using SaasFactory.Modules.Common;
using SaasFactory.ServiceDefaults;
using SaasFactory.Shared.Config;
using SaasFactory.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .UseSharedConfiguration()
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

var featureHubConfig = new EdgeFeatureHubConfig(
    Environment.GetEnvironmentVariable("FEATUREHUB_EDGE_URL"), 
    Environment.GetEnvironmentVariable("FEATUREHUB_API_KEY")
); 

var eventsDbConnectionString = 
    Environment.GetEnvironmentVariable("EVENTS_DB_CONNECTION") 
    ?? throw new InvalidOperationException("Missing Postgres connection string");

// connect and start listening, if this line hangs it's likely connectivity issues.
await featureHubConfig.Init(); // Connect and start listening
var featureHubCtx = await featureHubConfig.NewContext().Build();

List<IFeatureModule> featureModules = [
    new BankAccountModule(featureHubCtx),
    new RestaurantMenuModule(featureHubCtx)
];

await builder
    .AddLoggingConfiguration()
    .AddServiceDefaults()
    .AddEventStore(eventsDbConnectionString)
    .AddMessaging(featureModules)
    .AddFeatureModules(featureModules);

builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();

await app.AddFeatureMiddleware(featureModules);
await app.RunAsync();
