namespace SaasFactory.WebApi.Models;

public record DepositModel()
{
    public required string AccountId { get; set; }
    public required decimal Amount { get; set; }
}

