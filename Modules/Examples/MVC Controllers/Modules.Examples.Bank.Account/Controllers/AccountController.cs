using Marten;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Modules.Examples.Bank.Account.Commands;
using Modules.Examples.Bank.Account.Events;
using Modules.Examples.Bank.Account.Models;
using SaasFactory.EventSourcing.Contracts;

namespace Modules.Examples.Bank.Account.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountController(
    IBus bus,
    IEventStore eventStore,
    IDocumentStore documentStore,
    ILogger<AccountController> logger) : ControllerBase
{
    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] DepositCommand command)
    {
        logger.LogDebug("Deposit command received");
       
        if (!BankAccountModule.IsEnabled)
        {
            logger.LogWarning("Bank account module is not enabled");
            return NotFound();
        }

        // Log an event to store the action

        var eventModel = DepositPendingEvent.From(command);
        await eventStore.AddEventPendingAsync(eventModel);
        logger.LogDebug("Deposit pending");

        if (string.IsNullOrWhiteSpace(command.AccountId))
            return CreateBadRequest("No account id provided", command);

        if (command.Amount <= 0)
            return CreateBadRequest("Deposit amount must be positive", command);

        await bus.Publish(command);

        // return
        return Accepted();
    }

    [HttpGet("{accountId}/balance")]
    public async Task<IActionResult> GetBalance(string accountId)
    {
        logger.LogDebug("Getting account balance.");
        
        if (!BankAccountModule.IsEnabled)
        {
            logger.LogWarning("Bank account module is not enabled");
            return NotFound();
        }
        
        // access the pre-built AccountState document directly
        await using var session = documentStore.QuerySession();
        var model = await session.LoadAsync<AccountState>(accountId);

        // validate 
        if (model == null) return NotFound();

        // return
        return Ok(model);
    }

    private IActionResult CreateBadRequest(string message, DepositCommand command)
    {
        var eventModel = DepositFailedEvent.From(command);
        logger.LogError("Bad request:{0}", message);
        // Fire and forget but retain exception data.
        _ = Task.Run(async void () =>
        {
            try
            {
                await eventStore.AddEventFailedAsync(eventModel);
            }
            catch (Exception ex)
            {
                logger.LogCritical("Failed to add event failed to transaction log: {0}", ex);
            }
        });

        return BadRequest(message);
    }
}