using System.Data;
using Marten;
using Mediator.Net;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SaasFactory.Features.UserSubscriptions.Contracts.Commands;
using SaasFactory.Features.UserSubscriptions.Contracts.Events;
using Serilog;

namespace SaasFactory.Features.UserSubscriptions;

public static class SubscriptionEndpoints
{

    public static void MapEndpoints(this IEndpointRouteBuilder routes, ILogger logger)
    {
        var group = routes
            .MapGroup("/subscribe")
            .RequireAuthorization(); // ensures a valid JWT is present

        group.MapGet("/", async (HttpContext ctx, [FromServices] IMediator mediator) =>
        {
            var user = ctx.User;
            var username = user.Identity?.Name
                           ?? user.FindFirst("preferred_username")?.Value
                           ?? "unknown";

            await mediator.SendAsync(new InitiateSubscriptionCommand(username));
            logger.Information("User {0}, made a subscription request", username);
            
            return Results.Ok($"User {username}, subscribed to thing");
        });
    }
}

public class SubscribeUserCommandHandler(ILogger logger, IDocumentStore store) : ICommandHandler<InitiateSubscriptionCommand>
{
    public async Task Handle(IReceiveContext<InitiateSubscriptionCommand> context, CancellationToken cancellationToken)
    {
        var transactionId = Guid.CreateVersion7();
        await using var session = store.LightweightSession(IsolationLevel.ReadCommitted);
        var subscriptionPendingEvent = new SubscriptionPendingEvent();
        session.Events.StartStream(transactionId, subscriptionPendingEvent);
        await session.SaveChangesAsync(cancellationToken);
        logger.Information("User Sub");
    }
}