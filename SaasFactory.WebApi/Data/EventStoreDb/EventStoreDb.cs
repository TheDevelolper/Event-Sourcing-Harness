using SaasFactory.WebApi.Data.EntityFramework;
using SaasFactory.WebApi.Events;
using SaasFactory.WebApi.Models;

namespace SaasFactory.WebApi.Data.EventStoreDb;

public class EventStoreDb : IEventStore
{
    public async Task AddDepositEventAsync(DepositEventPayload deposit, StatusEnum status)
    {
        //throw new NotImplementedException();
    }
}