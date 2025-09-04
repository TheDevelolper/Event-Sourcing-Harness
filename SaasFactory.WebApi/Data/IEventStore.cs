using SaasFactory.WebApi.Events;

namespace SaasFactory.WebApi.Data;

public interface IEventStore
{
    public Task AddEventPendingAsync(PendingEvent eventModel);
    public Task AddEventCompletedAsync(CompletedEvent eventModel);
    public Task<T?> ProjectAsync<T>(string id) where T : notnull;
}