
using Mediator.Net.Contracts;

namespace SaasFactory.Features.UserSubscriptions.Contracts.Commands;

/// <summary>
/// Command used to create a new subscription.
/// </summary>
/// <param name="userId">The identifier of the user initiating the subscription.</param>
// ReSharper disable once ClassNeverInstantiated.Global
public record InitiateSubscriptionCommand(string userId) : ICommand;
