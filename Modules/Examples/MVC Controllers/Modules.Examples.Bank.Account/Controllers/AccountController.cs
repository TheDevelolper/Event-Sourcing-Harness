using Marten;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Modules.Examples.Bank.Account.Commands;
using Modules.Examples.Bank.Account.Events;
using Modules.Examples.Bank.Account.Models;
using SaasFactory.EventSourcing.Contracts;

namespace Modules.Examples.Bank.Account.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountController : ControllerBase
{
    private readonly IBus _bus;
    private readonly IEventStore _eventStore;
    private readonly IDocumentStore _documentStore;

    public AccountController(IBus bus, IEventStore eventStore, IDocumentStore documentStore)
    {
        _bus = bus;
        _eventStore = eventStore;
        _documentStore = documentStore;
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] DepositCommand command)
    {
        if (!BankAccountModule.IsEnabled) return NotFound();
        
        // Log an event to store the action
        var eventModel = DepositPendingEvent.From(command);
        await _eventStore.AddEventPendingAsync(eventModel);
       
        if(string.IsNullOrWhiteSpace(command.AccountId)) 
            return CreateBadRequest("No account id provided", command);
        
        if(command.Amount <= 0)
            return CreateBadRequest("Deposit amount must be positive", command);
        
        await _bus.Publish(command);
        
        // return
        return Accepted();
    }
    
    [HttpGet("{accountId}/balance")]
    public async Task<IActionResult> GetBalance(string accountId)
    {
        if (!BankAccountModule.IsEnabled) return NotFound();
        
        // access the pre-built AccountState document directly
        await using var session = _documentStore.QuerySession();
        var model = await session.LoadAsync<AccountState>(accountId);
        
        // validate 
        if(model == null) return NotFound();
        
        // return
        return Ok(model);
    }
    
    private IActionResult CreateBadRequest(string message, DepositCommand command)
    {
        var eventModel = DepositFailedEvent.From(command);

        // Fire and forget but retain exception data.
        _ = Task.Run(async void () =>
        {
            try
            {
                await _eventStore.AddEventFailedAsync(eventModel);
            }
            catch (Exception ex)
            {
                // todo: need to use ILogger here once logging is set up properly.
                // log it, swallow it, or handle it
                Console.Error.WriteLine("Failed to add event failed to transaction log: {0}", ex);
            }
        });
        
        return BadRequest(message);
    }

}