using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Serilog;

namespace SaasFactory.Features.UserSubscriptions;

public static class SubscriptionEndpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder routes, ILogger logger)
    {
        var group = 
            routes.MapGroup("/subscribe")
                .RequireAuthorization(); // ensures a valid JWT is present

        group.MapGet("/", (HttpContext ctx) =>
        {
            var user = ctx.User;
            var username = user.Identity?.Name 
                           ?? user.FindFirst("preferred_username")?.Value 
                           ?? "unknown";
            
            logger.Information("User {0}, subscribed to thing", username);
            return Results.Ok($"User {username}, subscribed to thing");
        });
    }
}
