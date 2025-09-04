using Marten;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SaasFactory.WebApi.Data;
using SaasFactory.WebApi.Events;
using SaasFactory.WebApi.Models;
using SaasFactory.WebApi.Projections;

namespace SaasFactory.WebApi.Controllers;

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
    public async Task<IActionResult> Deposit([FromBody] DepositModel deposit)
    {
        // Define a domain event (plain class, no wrapping needed)
        var eventModel = DepositPendingEvent.From(deposit);
        await _eventStore.AddEventPendingAsync(eventModel);
        
        // access
        await _bus.Publish(deposit);
        
        // return
        return Accepted();
    }

    [HttpGet("{accountId}/balance")]
    public async Task<IActionResult> GetBalance(string accountId)
    {
        // access the pre-built AccountState document directly
        await using var session = _documentStore.QuerySession();
        var model = await session.LoadAsync<AccountState>(accountId);
        
        // validate 
        if(model == null) return NotFound();
        
        // return
        return Ok(model);
    }
}