namespace SaasFactory.WebApi.Events;

public record DepositEventPayload(Guid Id, string AccountId, decimal Amount);
