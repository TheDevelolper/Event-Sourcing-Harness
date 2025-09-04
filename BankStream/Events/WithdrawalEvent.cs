namespace BankStream.Events;

public record WithdrawalEvent(Guid Id, string AccountId, decimal Amount);