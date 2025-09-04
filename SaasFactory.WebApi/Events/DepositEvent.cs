namespace SaasFactory.Events;

public record DepositEvent(Guid Id, string AccountId, decimal Amount);
