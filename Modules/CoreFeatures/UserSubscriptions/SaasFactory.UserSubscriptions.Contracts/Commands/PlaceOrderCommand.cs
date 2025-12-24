namespace SaasFactory.UserSubscriptions.Contracts.Commands;

/// <summary>
/// Command to place an order as part of the subscription process.
/// </summary>
public record PlaceOrderCommand(List<string>? stream): BaseCommand(stream);