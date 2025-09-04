using BankStream.Data;
using BankStream.Events;
using BankStream.Models;
using MassTransit;

namespace BankStream.Consumers;

public class DepositConsumer : IConsumer<DepositEvent>
{
    private readonly EventDbContext _dbContext;

    public DepositConsumer(EventDbContext dbContext) => _dbContext = dbContext;

    public async Task Consume(ConsumeContext<DepositEvent> context)
    {
        var deposit = context.Message;

        try
        {
            await _dbContext.TransactionStatus.AddAsync(new TransactionStatus(
                Guid.NewGuid(),
                deposit.AccountId,
                StatusEnum.Completed,
                deposit.Amount,
                true,
                string.Empty,
                TransactionTypeEnum.Deposit,
                DateTime.Now)
            );

            await _dbContext.Deposits.AddAsync(deposit);

            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            await _dbContext.TransactionStatus.AddAsync(new TransactionStatus(
                Guid.NewGuid(),
                deposit.AccountId,
                StatusEnum.Failed,
                deposit.Amount,
                true,
                ex.Message,
                TransactionTypeEnum.Deposit,
                DateTime.Now)
            );
            await _dbContext.SaveChangesAsync();
            throw;
        }
    }
}