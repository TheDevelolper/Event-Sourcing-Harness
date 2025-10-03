using MassTransit;
using Microsoft.Extensions.Hosting;
using SaasFactory.Modules.Common;

namespace SaasFactory.Messaging.MasTransit;

public static class BuilderExtensions
{
    public static IHostApplicationBuilder AddMessaging(
        this IHostApplicationBuilder builder, 
        IEnumerable<IFeatureModule> featureModules)
    {
        builder.Services.AddMassTransit(options =>
        {
            foreach (var messageConsumerModule in featureModules)
            {
                messageConsumerModule.RegisterMessageConsumers(options);
            }
            
            options.SetKebabCaseEndpointNameFormatter();

            // Core transport setup
            options.UsingRabbitMq((context, cfg) =>
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