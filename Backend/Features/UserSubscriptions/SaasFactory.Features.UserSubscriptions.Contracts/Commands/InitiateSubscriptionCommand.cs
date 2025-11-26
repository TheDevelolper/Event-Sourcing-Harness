using Mediator.Net.Contracts;

namespace SaasFactory.Features.UserSubscriptions.Contracts.Commands;

/// <summary>
/// Command used to create a new subscription.
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public record InitiateSubscriptionCommand(string userId, List<string>? stream): BaseCommand(stream);

/// <summary>
/// Base command.
/// </summary>
/// <param name="userId">The identifier of the user initiating the subscription.</param>
public abstract record BaseCommand(List<string>? stream = null): ICommand;