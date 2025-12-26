namespace SaasFactory.EventSourcing.Contracts;

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

public class FailedEvent : IEvent
{
    public EventStatus Status { get; } =  EventStatus.Failed;
}