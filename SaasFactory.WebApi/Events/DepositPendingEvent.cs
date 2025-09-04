using SaasFactory.WebApi.Models;

namespace SaasFactory.WebApi.Events;

public record DepositPendingEvent (string AccountId, decimal Amount): PendingEvent
{
    public static DepositPendingEvent From(DepositModel depositModel)
    {
        return new DepositPendingEvent(depositModel.AccountId, depositModel.Amount);
    }
}
