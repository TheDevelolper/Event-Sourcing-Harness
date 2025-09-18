using MassTransit;
using Modules.Examples.Bank.Account.Commands;
using Modules.Examples.Bank.Account.Events;
using SaasFactory.EventSourcing.Contracts;

namespace Modules.Examples.Bank.Account.Consumers;

// ReSharper disable once ClassNeverInstantiated.Global
public class DepositConsumer(IEventStore eventStore) : IConsumer<DepositCommand>
{
    public async Task Consume(ConsumeContext<DepositCommand> context)
    {
        var deposit = context.Message;
        var eventModel = DepositCompletedEvent.From(deposit);
        await eventStore.AddEventCompletedAsync(eventModel);
    }
}