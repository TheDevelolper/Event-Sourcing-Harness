namespace SaasFactory.WebApi.Events;

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
