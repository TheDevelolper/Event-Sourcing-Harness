using SaasFactory.EventSourcing.Contracts;

namespace SaasFactory.Features.UserSubscriptions.Contracts.Events;

/// <summary>
/// Event raised when an order has been placed
/// as part of the subscription process.
/// </summary>
public record OrderPendingEvent : PendingEvent;