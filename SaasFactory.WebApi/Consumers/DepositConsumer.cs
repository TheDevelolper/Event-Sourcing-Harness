using MassTransit;
using SaasFactory.Shared.Config;
using SaasFactory.WebApi.Data;
using SaasFactory.WebApi.Events;
using SaasFactory.WebApi.Models;

namespace SaasFactory.WebApi.Consumers;

public class DepositConsumer(IEventStore eventStore) : IConsumer<DepositEventPayload>
{
    public async Task Consume(ConsumeContext<DepositEventPayload> context)
    {
        var deposit = context.Message;
        await eventStore.AddDepositEventAsync(deposit, StatusEnum.Completed);
    }
}