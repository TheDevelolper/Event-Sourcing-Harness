using Marten.Events.Projections;
using Modules.Examples.Bank.Account.Events;
using Modules.Examples.Bank.Account.Models;

namespace Modules.Examples.Bank.Account.Projections;

public class AccountStateProjection : MultiStreamProjection<AccountState, string>
{
    public AccountStateProjection()
    {
        // This tells Marten to group events by the 'AccountId' property
        // and project them into a document with that same Id.
        Identity<DepositCompletedEvent>(x => x.AccountId);
    }

    // This method is used to CREATE the aggregate from the first event.
    // It takes the event as a parameter and returns a new instance of the aggregate.
    public AccountState Create(DepositCompletedEvent deposit)
    {
        return new AccountState
        {
            Id = deposit.AccountId,
            Balance = deposit.Amount,
            NumberOfDeposits = 1
        };
    }

    // Marten will call this to apply a subsequent event to an existing
    // AccountState document.
    public void Apply(AccountState account, DepositCompletedEvent deposit)
    {
        account.Balance += deposit.Amount;
        account.NumberOfDeposits++;
    }
}
