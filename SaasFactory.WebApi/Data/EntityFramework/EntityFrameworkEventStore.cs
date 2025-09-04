using SaasFactory.WebApi.Controllers;
using SaasFactory.WebApi.Events;
using SaasFactory.WebApi.Models;

namespace SaasFactory.WebApi.Data.EntityFramework;

public class EntityFrameworkEventStore(EventDbContext dbContext) : IEventStore
{
    public async Task AddDepositEventAsync(DepositEventPayload deposit, StatusEnum status)
    {
        var payload = System.Text.Json.JsonSerializer.Serialize(deposit);
        var transactionStatus = new TransactionStatus(
                Guid.NewGuid(),
                deposit.AccountId,
                status,
                deposit.Amount,
                payload, 
                true,
                string.Empty,
                TransactionTypeEnum.Deposit,
                DateTime.Now
            );
        
        try
        {
            await dbContext.TransactionStatus.AddAsync(transactionStatus);
            if (status is StatusEnum.Completed)
            {
                await dbContext.Deposits.AddAsync(deposit);
            }
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            await HandleTransactionFailure(transactionStatus, ex.Message);
            throw;
        }
    }
    
    private async Task HandleTransactionFailure(TransactionStatus transactionStatus, string errorMessage)
    {
        transactionStatus.IsSuccess = false;
        transactionStatus.StatusEnum = StatusEnum.Failed;
        transactionStatus.ErrorMessage = errorMessage;
        await dbContext.TransactionStatus.AddAsync(transactionStatus);
        await dbContext.SaveChangesAsync();
    }
}