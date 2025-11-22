
using Mediator.Net.Contracts;

namespace SaasFactory.Features.UserSubscriptions.Contracts.Commands;

/// <summary>
/// Command used to create a new subscription.
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public record InitiateSubscriptionCommand(string userId) : ICommand;