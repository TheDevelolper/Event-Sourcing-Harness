
using Mediator.Net.Contracts;

namespace SaasFactory.Features.UserSubscriptions.Contracts.Commands;

/// <summary>
/// Command to place an order as part of the subscription process.
/// </summary>
public record PlaceOrderCommand() : ICommand;