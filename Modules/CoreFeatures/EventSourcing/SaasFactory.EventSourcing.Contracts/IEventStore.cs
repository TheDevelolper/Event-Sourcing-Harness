namespace SaasFactory.EventSourcing.Contracts;

public interface IEventStore
{
    public Task AddEventPendingAsync(PendingEvent eventModel);
    public Task AddEventCompletedAsync(CompletedEvent eventModel);
    
    public Task AddEventFailedAsync(FailedEvent eventModel);
    
    public Task<T?> ProjectAsync<T>(string id) where T : notnull;

}