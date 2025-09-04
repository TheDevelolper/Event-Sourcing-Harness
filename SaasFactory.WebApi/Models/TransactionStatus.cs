namespace SaasFactory.Models;

public class TransactionStatus
{
    public TransactionStatus()
    {
    }

    public TransactionStatus(Guid id, string accountId, StatusEnum statusEnum, decimal amount, bool isSuccess, string? errorMessage, TransactionTypeEnum type, DateTime createdAt)
    {
        Id = id;
        AccountId = accountId;
        StatusEnum = statusEnum;
        Amount = amount;
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        Type = type;
        CreatedAt = createdAt;
    }

    public Guid Id { get; set; }
    public string AccountId { get; set; }
    public StatusEnum StatusEnum { get; set; }
    public decimal Amount { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public TransactionTypeEnum Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}