using System.Data;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using JasperFx.Events;
using Marten;
using Mediator.Net;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SaasFactory.UserSubscriptions.Contracts.Commands;
using SaasFactory.UserSubscriptions.Contracts.Events;
using Serilog;


namespace SaasFactory.UserSubscriptions;

// Todo: Versioning
// Todo: Validation
// Todo: Error Handling

public static class SubscriptionEndpoints
{

    public static void MapEndpoints(this IEndpointRouteBuilder routes, ILogger logger)
    {
        var group = routes
            .MapGroup("/subscribe")
            .RequireAuthorization(); // ensures a valid JWT is present


        group.MapGet("/", async (HttpContext ctx, [FromServices] IMediator mediator) =>
        {
            var resultStream = new List<string>();
            var user = ctx.User;
            var username = user.Identity?.Name
                        ?? user.FindFirst("preferred_username")?.Value
                        ?? "unknown";
            
            // tests depend on this log statement.
            logger.Information("User {0}, made a subscription request", username);
            await mediator.SendAsync(new InitiateSubscriptionCommand(username, resultStream));
 
            return GetResultStream(resultStream);

            async IAsyncEnumerable<string> GetResultStream(List<string> s)
            {
                foreach (var msg in s)
                    yield return msg;
            }

        }).Produces(statusCode: 200, contentType: MediaTypeNames.Text.EventStream);

    }
}

public class SubscribeUserCommandHandler(ILogger logger, IMediator mediator, IDocumentStore store) : ICommandHandler<InitiateSubscriptionCommand>
{
    public async Task Handle(IReceiveContext<InitiateSubscriptionCommand> context, CancellationToken cancellationToken)
    {
        var transactionId = Guid.CreateVersion7();
        await using var session = store.LightweightSession(IsolationLevel.ReadCommitted);
        session.Events.StartStream(transactionId, new SubscriptionPendingEvent());
        await session.SaveChangesAsync(cancellationToken);

        context.Message.stream?
            .Add($"data: SubscriptionPendingEvent for transaction {transactionId}\n\n");

        await mediator.SendAsync(new PlaceOrderCommand(context.Message.stream), cancellationToken);
        logger.Information("Placed order for subscription");
    }
}

public class ProcessOrderCommandHandler(ILogger logger) : ICommandHandler<PlaceOrderCommand>
{
    public Task Handle(IReceiveContext<PlaceOrderCommand> context, CancellationToken cancellationToken)
    {
        logger.Information("Processing order...");

        context.Message.stream?
            .Add("data: PlaceOrderCommand received\n\n");
            
        return Task.CompletedTask; 
    }
}

public abstract class SmartEnum(string defaultValue)
{
    protected string Value { get; set; } = defaultValue;
    public override string ToString() => Value;
}
public class SubscriptionState: SmartEnum
{
    public static SubscriptionState Unsubscribed => new(nameof(Unsubscribed));
    public static SubscriptionState Pending => new(nameof(Pending));
    public static SubscriptionState Active => new(nameof(Active));

    private SubscriptionState(string value): base(value)
    {
        Value = value;
    }
}

