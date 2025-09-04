using SaasFactory.WebApi.Events;
using SaasFactory.WebApi.Models;

namespace SaasFactory.WebApi.Data;

public interface IEventStore
{
    Task AddDepositEventAsync(DepositEventPayload deposit, StatusEnum status);
}