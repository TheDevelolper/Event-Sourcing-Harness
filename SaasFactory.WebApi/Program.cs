using Modules.Examples.Bank.Account;
using Modules.Examples.Restaurant.Menu;
using SaasFactory.EventSourcing.Marten;
using SaasFactory.Messaging.MasTransit;
using SaasFactory.ServiceDefaults;
using SaasFactory.Shared.Config;
using SaasFactory.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .UseSharedConfiguration()
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

var eventsDbConnectionString = 
    Environment.GetEnvironmentVariable("EVENTS_DB_CONNECTION") 
    ?? throw new InvalidOperationException("Missing Postgres connection string");

builder
    .AddLogging()
    .AddEventStore(eventsDbConnectionString)
    .AddBankAccountModule()
    .AddRestaurantMenuModuleExample()
    .AddMessaging(options =>
    {
        // ReSharper disable once ConvertClosureToMethodGroup
        BankAccountModule.AddMessageConsumers(options);
        // Add your own message consumers here!
        // YourModule.AddMessageConsumers(options);
    })
    .AddServiceDefaults();
    

builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();
app.UseRestaurantMenuModuleExample(); //👈 init endpoints + middleware etc
app.Run();