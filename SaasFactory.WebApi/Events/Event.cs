namespace SaasFactory.WebApi.Events;

public interface IEvent
{
    public EventStatus Status { get; }
}

public record PendingEvent() : IEvent
{
    public EventStatus Status { get; } =  EventStatus.Pending;
}

public class CompletedEvent : IEvent
{
    public EventStatus Status { get; } =  EventStatus.Completed;
}