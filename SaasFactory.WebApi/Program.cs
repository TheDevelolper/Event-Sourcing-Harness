using SaasFactory.Consumers;
using SaasFactory.Data;
using SaasFactory.ServiceDefaults;
using SaasFactory.Shared.Config;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration
    .UseSharedConfiguration()
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

var featureToggles = new FeatureToggles();
configuration.GetSection("toggles").Bind(featureToggles);

builder
    .Services
    .AddDbContext<EventDbContext>(options => options.UseSqlite("Data Source=events.db"));

// builder.AddDefaultHealthChecks();
builder.AddServiceDefaults();




if (featureToggles.UseRabbitMq)
{
    builder
        .Services
        .AddMassTransit(x =>
        {
            x.AddConsumer<DepositConsumer>();
            x.AddConsumer<WithdrawalConsumer>();
            x.UsingRabbitMq(
                (context, cfg) =>
                {
                    cfg.Host("rabbitmq://localhost");
                    cfg.ReceiveEndpoint(
                        "event-queue",
                        e =>
                        {
                            e.SetQueueArgument("x-message-ttl", 500000);
                            e.ConfigureConsumer<DepositConsumer>(context);
                            e.ConfigureConsumer<WithdrawalConsumer>(context);
                        }
                    );
                }
            );
        });
}


builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();