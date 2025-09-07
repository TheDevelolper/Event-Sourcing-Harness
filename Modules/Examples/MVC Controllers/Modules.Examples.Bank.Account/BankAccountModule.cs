using JasperFx.Events.Projections;
using Marten;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Modules.Examples.Bank.Account.Consumers;
using Modules.Examples.Bank.Account.Events;
using Modules.Examples.Bank.Account.Projections;

namespace Modules.Examples.Bank.Account;

public static class BankAccountModule
{
    
    public static IHostApplicationBuilder AddBankAccountModule(this IHostApplicationBuilder builder)
    {
        builder.Services.ConfigureMarten(options =>
        {
            options.Projections.Add<AccountStateProjection>(ProjectionLifecycle.Async);
            options.Events.AddEventType(typeof(DepositCompletedEvent));
        });
     
        return builder;
    }
    
    public static void AddMessageConsumers(IBusRegistrationConfigurator x)
    {
        x.AddConsumer<DepositConsumer>();
    }
}