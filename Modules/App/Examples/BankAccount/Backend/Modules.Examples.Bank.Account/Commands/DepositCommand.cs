namespace Modules.Examples.Bank.Account.Commands;

public record DepositCommand()
{
    public required string AccountId { get; set; }
    public required decimal Amount { get; set; }
}

