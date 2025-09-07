using MassTransit;
using Microsoft.Extensions.Hosting;

namespace SaasFactory.Messaging.MasTransit;

public static class BuilderExtensions
{
    public static IHostApplicationBuilder AddMessaging(this IHostApplicationBuilder builder,Action<IBusRegistrationConfigurator> configure)
    {
        builder.Services.AddMassTransit(x =>
        {
            configure(x);
            
            x.SetKebabCaseEndpointNameFormatter();

            // Core transport setup
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq://localhost");

                cfg.ReceiveEndpoint("event-queue", e =>
                {
                    e.SetQueueArgument("x-message-ttl", 500000);
                    e.ConfigureConsumers(context); // 👈 configure *all* registered consumers
                });
            });
        });

        return builder;
    }
}