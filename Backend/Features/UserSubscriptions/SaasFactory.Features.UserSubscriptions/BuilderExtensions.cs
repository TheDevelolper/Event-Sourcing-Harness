using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SaasFactory.Features.UserSubscriptions;

public static class BuilderExtensions
{
    public static IHostApplicationBuilder AddUserSubscriptionServices(this IHostApplicationBuilder builder)
    {
        // services go here
        return builder;
    }
    
    public static void MapUserSubscriptionEndpoints(this IEndpointRouteBuilder routes, ILogger logger)
    {
        logger.Information("Adding UserSubscription endpoints");
        routes.MapEndpoints(logger);
        logger.Information("UserSubscription endpoints mapped");
    }
}