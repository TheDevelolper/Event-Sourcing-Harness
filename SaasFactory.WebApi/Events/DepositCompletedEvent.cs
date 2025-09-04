using SaasFactory.WebApi.Models;

namespace SaasFactory.WebApi.Events;

public class DepositCompletedEvent: CompletedEvent
{
    public required decimal Amount { get; init; }
    public required string AccountId { get; init; }
    
    public static DepositCompletedEvent From(DepositModel deposit)
    {
        return new DepositCompletedEvent
        {
            AccountId = deposit.AccountId,
            Amount = deposit.Amount,
        };
    }
}
