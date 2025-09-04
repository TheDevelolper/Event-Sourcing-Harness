using SaasFactory.WebApi.Events;
using Marten.Events.Projections;

namespace SaasFactory.WebApi.Projections;

// This will be a Marten document, stored in a separate table.
public class AccountState
{
    public string Id { get; set; } // The AccountId serves as the primary key
    public decimal Balance { get; set; }
    public int NumberOfDeposits { get; set; }
}

// The key here is to inherit from MultiStreamProjection, and specify your
// aggregate type.
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