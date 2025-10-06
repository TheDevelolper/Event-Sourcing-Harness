namespace Modules.Examples.Bank.Account.Models;

public class AccountState
{
    //todo: consider making Id required
    public required string Id { get; set; } // The AccountId serves as the primary key
    public decimal Balance { get; set; }
    public int NumberOfDeposits { get; set; }
}