using SaasFactory.ServiceDefaults;
using SaasFactory.Shared.Config;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using SaasFactory.WebApi.Consumers;
using SaasFactory.WebApi.Data.EntityFramework;
using SaasFactory.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration
    .UseSharedConfiguration()
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

var featureToggles = FeatureToggles.FromConfig(configuration);

builder.Services.AddDbContext<EventDbContext>(options => options.UseSqlite("Data Source=events.db"));
builder.AddEventStore(featureToggles.UseEventStore);

if (featureToggles.UseRabbitMq)
{
    builder.Services
        .AddMassTransit(x =>
        {
            x.AddConsumer<DepositConsumer>();
            x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq://localhost");
                    cfg.ReceiveEndpoint(
                        "event-queue",
                        e =>
                        {
                            e.SetQueueArgument("x-message-ttl", 500000);
                            e.ConfigureConsumer<DepositConsumer>(context);
                        }
                    );
                }
            );
        });
}

builder.AddServiceDefaults();
builder.Services.AddControllers();

var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();
var eventDbContext = scope.ServiceProvider.GetRequiredService<EventDbContext>();
await eventDbContext.Database.MigrateAsync();

app.MapControllers();
app.Run();