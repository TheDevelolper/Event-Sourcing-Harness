namespace SaasFactory.Events;

public record WithdrawalEvent(Guid Id, string AccountId, decimal Amount);