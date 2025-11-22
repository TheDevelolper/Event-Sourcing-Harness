using Marten;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using SaasFactory.Features.UserSubscriptions.Contracts.Events;
using SaasFactory.Modules.Common;
using Serilog;

namespace SaasFactory.Features.UserSubscriptions;

public static class BuilderExtensions
{
    public static IServiceCollection AddUserSubscriptionServices(this IServiceCollection services)
    {
        var logger = CommonLoggerFactory.CreateLogger("User Subscription");
        
        // services go here
        services.AddSingleton<ILogger>(logger);
        
        services.ConfigureMarten(options =>
        {
            options.Events.AddEventType(typeof(SubscriptionPendingEvent));
        });
        
        return services;
    }
    
    public static IEndpointRouteBuilder MapUserSubscriptionEndpoints(this IEndpointRouteBuilder routes, ILogger logger)
    {
        logger.Information("Adding UserSubscription endpoints");
        routes.MapEndpoints(logger);
        logger.Information("UserSubscription endpoints mapped");
        return routes;
    }
}