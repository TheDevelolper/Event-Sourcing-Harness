using BankStream.Data;
using BankStream.Events;
using BankStream.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace BankStream.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountController : ControllerBase
{
    private readonly IBus _bus;
    private readonly EventDbContext _dbContext;

    public AccountController(IBus bus, EventDbContext dbContext)
    {
        _bus = bus;
        _dbContext = dbContext;
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] DepositEvent deposit)
    {
        await _dbContext.TransactionStatus.AddAsync(new TransactionStatus(
            Guid.NewGuid(),
            deposit.AccountId,
            StatusEnum.Pending,
            deposit.Amount,
            false,
            string.Empty,
            TransactionTypeEnum.Deposit,
            DateTime.Now)
        );
        await _dbContext.SaveChangesAsync();

        await _bus.Publish(deposit);
        return Accepted();
    }

    [HttpPost("withdrawal")]
    public async Task<IActionResult> Withdraw([FromBody] WithdrawalEvent withdrawal)
    {
        await _dbContext.TransactionStatus.AddAsync(new TransactionStatus(
            Guid.NewGuid(),
            withdrawal.AccountId,
            StatusEnum.Pending,
            withdrawal.Amount,
            false,
            string.Empty,
            TransactionTypeEnum.Withdrawal,
            DateTime.Now)
         );
        await _dbContext.SaveChangesAsync();

        await _bus.Publish(withdrawal);
        return Accepted();
    }

    [HttpGet("{accountId}/balance")]
    public IActionResult GetBalance(string accountId)
    {
        var deposits = _dbContext.
            Deposits.
            Where(d => d.AccountId == accountId)
            .Sum(d => d.Amount);

        var withdrawals = _dbContext
            .Withdrawals
            .Where(w => w.AccountId == accountId)
            .Sum(w => w.Amount);

        return Ok(deposits - withdrawals);
    }
}