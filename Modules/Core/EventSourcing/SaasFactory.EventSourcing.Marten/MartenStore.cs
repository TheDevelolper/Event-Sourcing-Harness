using Marten;
using SaasFactory.EventSourcing.Contracts;

namespace SaasFactory.EventSourcing.Marten;

public class MartenStore(IDocumentStore store) : IEventStore
{
    public async Task AddEventPendingAsync(PendingEvent eventModel) 
        => await AddEventAsync(eventModel);
    public async Task AddEventCompletedAsync(CompletedEvent eventModel)
        => await AddEventAsync(eventModel);

    public async Task AddEventFailedAsync(FailedEvent eventModel) 
        => await AddEventAsync(eventModel);

    private async Task AddEventAsync(IEvent eventModel)
    {
        // Generate a new, unique ID for this specific deposit transaction
        var transactionId = Guid.CreateVersion7();

        await using var session = store.LightweightSession();
        session.Events.StartStream(transactionId, eventModel);
        await session.SaveChangesAsync();
    }

    public async Task<T?> ProjectAsync<T>(string id) where T : notnull
    {
        // Use a QuerySession for read-only operations. It is slightly more performant
        // than a LightweightSession if you only need to query data.
        await using var session = store.QuerySession();

        return await session.LoadAsync<T>(id);
    }
}