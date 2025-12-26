using SaasFactory.EventSourcing.Contracts;

namespace SaasFactory.UserSubscriptions.Contracts.Events;

/// <summary>
/// Event raised when a subscription creation process
/// has been initiated and is awaiting completion.
/// </summary>
public record SubscriptionPendingEvent : PendingEvent;
