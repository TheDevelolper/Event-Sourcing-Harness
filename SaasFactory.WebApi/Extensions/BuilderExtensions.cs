using JasperFx;
using JasperFx.Events.Daemon;
using JasperFx.Events.Projections;
using Marten;
using MassTransit;
using SaasFactory.WebApi.Consumers;
using SaasFactory.WebApi.Data;
using SaasFactory.WebApi.Data.Marten;
using SaasFactory.WebApi.Events;
using SaasFactory.WebApi.Projections;

namespace SaasFactory.WebApi.Extensions;

public static class BuilderExtensions
{
    public static IHostApplicationBuilder AddLogging(this IHostApplicationBuilder builder)
    {
        builder.Logging.AddConsole();
        builder.Logging.SetMinimumLevel(LogLevel.Debug);
        return builder;
    }
    
    public static IHostApplicationBuilder AddEventStore(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IEventStore, MartenStore>();
                
        var eventsConnectionString = 
            Environment.GetEnvironmentVariable("EVENTS_DB_CONNECTION") 
                ?? throw new InvalidOperationException("Missing Postgres connection string");

        builder.Services.AddMarten(options =>
        {
            options.Connection(eventsConnectionString);
            options.Projections.Add<AccountStateProjection>(ProjectionLifecycle.Async);
            options.Events.AddEventType(typeof(IEvent));
            options.Events.AddEventType(typeof(DepositCompletedEvent));
            options.UseSystemTextJsonForSerialization();
            options.AutoCreateSchemaObjects = AutoCreate.All;

        }).AddAsyncDaemon(DaemonMode.Solo);
        
        return builder;
    }
    
    public static IHostApplicationBuilder AddMessaging(this IHostApplicationBuilder builder)
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
        return builder;
    }
}