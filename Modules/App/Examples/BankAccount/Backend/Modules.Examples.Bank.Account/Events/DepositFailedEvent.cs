using Modules.Examples.Bank.Account.Commands;
using SaasFactory.EventSourcing.Contracts;

namespace Modules.Examples.Bank.Account.Events;

internal class DepositFailedEvent : FailedEvent
{
    public required DepositCommand Command { get; set; }

    public static DepositFailedEvent From(DepositCommand command)
    {
        var result = new DepositFailedEvent
        {
            Command = command,
        };

        return result;
    }
}