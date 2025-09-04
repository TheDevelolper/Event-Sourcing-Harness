using MassTransit;
using SaasFactory.Data;
using SaasFactory.Events;
using SaasFactory.Models;

namespace SaasFactory.Consumers;

public class WithdrawalConsumer : IConsumer<WithdrawalEvent>
{
    private readonly EventDbContext _dbContext;

    public WithdrawalConsumer(EventDbContext dbContext) => _dbContext = dbContext;

    public async Task Consume(ConsumeContext<WithdrawalEvent> context)
    {
        var withdrawal = context.Message;
        try
        {
            await _dbContext.TransactionStatus.AddAsync(new TransactionStatus(
                Guid.NewGuid(),
                withdrawal.AccountId,
                StatusEnum.Completed,
                withdrawal.Amount,
                true,
                string.Empty,
                TransactionTypeEnum.Withdrawal,
                DateTime.Now)
            );

            await _dbContext.Withdrawals.AddAsync(context.Message);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            await _dbContext.TransactionStatus.AddAsync(new TransactionStatus(
                Guid.NewGuid(),
                withdrawal.AccountId,
                StatusEnum.Failed,
                withdrawal.Amount,
                false,
                ex.Message,
                TransactionTypeEnum.Withdrawal,
                DateTime.Now)
            );
            await _dbContext.SaveChangesAsync();
            throw;
        }
    }
}