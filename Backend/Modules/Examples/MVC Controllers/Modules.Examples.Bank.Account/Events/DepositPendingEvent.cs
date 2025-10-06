using Modules.Examples.Bank.Account.Commands;
using SaasFactory.EventSourcing.Contracts;

namespace Modules.Examples.Bank.Account.Events;

public record DepositPendingEvent (string AccountId, decimal Amount): PendingEvent
{
    public static DepositPendingEvent From(DepositCommand depositCommand)
    {
        return new DepositPendingEvent(depositCommand.AccountId, depositCommand.Amount);
    }
}
