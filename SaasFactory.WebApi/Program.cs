using SaasFactory.ServiceDefaults;
using SaasFactory.Shared.Config;
using SaasFactory.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .UseSharedConfiguration()
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

builder
    .AddLogging()
    .AddEventStore()
    .AddMessaging()
    .AddServiceDefaults();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();