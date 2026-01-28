using JasperFx.Events.Projections;
using Marten;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modules.Examples.Bank.Account.Events;
using Modules.Examples.Bank.Account.Options;
using Modules.Examples.Bank.Account.Projections;
using SaasFactory.Modules.Common;

namespace Modules.Examples.Bank.Account;


public static class BankAccountConfig
{
    public const string SectionName = "Modules:Examples:BankAccount";
}

public class BankAccountModule: IFeatureModule
{
    public Task<IHostApplicationBuilder> AddModule(IHostApplicationBuilder builder)
    {
        builder.Services.AddOptions<BankAccountModuleOptions>()
            .BindConfiguration(BankAccountConfig.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.ConfigureMarten(options =>
        {
            options.Projections.Add<AccountStateProjection>(ProjectionLifecycle.Async);
            options.Events.AddEventType(typeof(DepositCompletedEvent));
        });
    
        return Task.FromResult(builder);
    }

    public Task<WebApplication> AddModuleMiddleware(WebApplication app) => Task.FromResult(app);
}

