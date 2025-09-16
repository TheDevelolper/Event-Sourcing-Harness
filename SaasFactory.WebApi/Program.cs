using Modules.Examples.Bank.Account;
using Modules.Examples.Restaurant.Menu;
using SaasFactory.EventSourcing.Marten;
using SaasFactory.Messaging.MasTransit;
using SaasFactory.ServiceDefaults;
using SaasFactory.Shared.Config;
using SaasFactory.WebApi.Extensions;

using FeatureHubSDK;
using JasperFx.CodeGeneration.Frames;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .UseSharedConfiguration()
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

var minimumLoggingLevel = LogLevel.Debug;
var logger = builder.CreateBootstrapLogger<Program>(minimumLoggingLevel);
builder.AddLogging(minimumLoggingLevel);

var eventsDbConnectionString = 
    Environment.GetEnvironmentVariable("EVENTS_DB_CONNECTION") 
    ?? throw new InvalidOperationException("Missing Postgres connection string");

// Uses environment variables
var featureHubConfig = new EdgeFeatureHubConfig(
    "http://localhost:4242", 
    "1c7229e2-a796-496f-8beb-0a19dbcfc684/7RaU5SQnFbP5KGFblCZeVI277tyBFidnWp6bGbfK"
); 

// connect and start listening, if this line hangs it's likely connectivity issues.
await featureHubConfig.Init(); // Connect and start listening
var featureHubCtx = await featureHubConfig.NewContext().Build();

var moduleFeature = featureHubCtx[nameof(BankAccountModule)];
BankAccountModule.IsEnabled = moduleFeature.BooleanValue ?? false;
moduleFeature.FeatureUpdateHandler += (sender, feature) =>
{
    var featureEnabled = feature.BooleanValue ?? false;
    var enabledStatus = featureEnabled ? "Enabled" : "Disabled";
    logger.LogInformation($"Bank Account Module {enabledStatus}");
    BankAccountModule.IsEnabled = featureEnabled;
};

builder
    .AddEventStore(eventsDbConnectionString)
    .AddRestaurantMenuModuleExample()
    .AddBankAccountModule()
    .AddMessaging(options =>
    {
        // ReSharper disable once ConvertClosureToMethodGroup
        BankAccountModule.AddMessageConsumers(options);
        // Add your own message consumers here! YourModule.AddMessageConsumers(options);
    })
    .AddServiceDefaults();

builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();

app.UseRestaurantMenuModuleExample(); //👈 init endpoints + middleware etc
await app.RunAsync();