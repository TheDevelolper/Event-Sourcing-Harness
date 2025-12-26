namespace SaasFactory.EventSourcing.Contracts;

public enum EventStatus
{
    Pending,
    Completed,
    Failed,
    Canceled,
    Reversed,
    InProgress,
    OnHold
}
