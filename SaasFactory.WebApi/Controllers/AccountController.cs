using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SaasFactory.WebApi.Data;
using SaasFactory.WebApi.Data.EntityFramework;
using SaasFactory.WebApi.Events;
using SaasFactory.WebApi.Models;

namespace SaasFactory.WebApi.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountController : ControllerBase
{
    private readonly IBus _bus;

    private readonly IEventStore _eventStore;
    // this is used for the projections
    // todo: perhaps wrap this up in a separate projections service. 
    private readonly EventDbContext _dbContext; 

    public AccountController(IBus bus, 
        IEventStore  eventStore, 
        EventDbContext dbContext
        )
    {
        _bus = bus;
        _dbContext = dbContext;
        _eventStore = eventStore;
        // _dbContext = dbContext;
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] DepositEventPayload deposit)
    {
        await _eventStore.AddDepositEventAsync(deposit, StatusEnum.InProgress);
        await _bus.Publish(deposit);
        return Accepted();
    }


    [HttpGet("{accountId}/balance")]
    public IActionResult GetBalance(string accountId)
    {
        var deposits = _dbContext.
            Deposits.
            Where(d => d.AccountId == accountId)
            .Sum(d => d.Amount);
  
        return Ok(deposits); 
    }
}

