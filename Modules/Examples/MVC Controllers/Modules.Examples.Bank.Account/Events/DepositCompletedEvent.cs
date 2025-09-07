using Modules.Examples.Bank.Account.Commands;
using SaasFactory.EventSourcing.Contracts;

namespace Modules.Examples.Bank.Account.Events;

public class DepositCompletedEvent: CompletedEvent
{
    public required decimal Amount { get; init; }
    public required string AccountId { get; init; }
    
    public static DepositCompletedEvent From(DepositCommand deposit)
    {
        return new DepositCompletedEvent
        {
            AccountId = deposit.AccountId,
            Amount = deposit.Amount,
        };
    }
}
