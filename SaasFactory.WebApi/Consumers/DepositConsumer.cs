using MassTransit;
using SaasFactory.WebApi.Data;
using SaasFactory.WebApi.Events;
using SaasFactory.WebApi.Models;

namespace SaasFactory.WebApi.Consumers;

public class DepositConsumer(IEventStore eventStore) : IConsumer<DepositModel>
{
    public async Task Consume(ConsumeContext<DepositModel> context)
    {
        var deposit = context.Message;
        var eventModel = DepositCompletedEvent.From(deposit);
        await eventStore.AddEventCompletedAsync(eventModel);
    }
}