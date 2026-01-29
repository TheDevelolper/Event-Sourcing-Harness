using JasperFx.Events.Projections;
using Marten;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modules.Examples.Bank.Account.Events;
using Modules.Examples.Bank.Account.Options;
using Modules.Examples.Bank.Account.Projections;
using SaasFactory.Modules.Common;
using Serilog.Core;

namespace Modules.Examples.Bank.Account;

public class BankAccountModule(Logger logger) : FeatureModule<BankAccountModuleOptions>(logger)
{
    protected override string ModuleName => nameof(BankAccountModule);

    public override Task<IHostApplicationBuilder> AddModule(IHostApplicationBuilder builder)
    {
        builder.Services.ConfigureMarten(options =>
        {
            options.Projections.Add<AccountStateProjection>(ProjectionLifecycle.Async);
            options.Events.AddEventType(typeof(DepositCompletedEvent));
        });
    
        return Task.FromResult(builder);
    }

    public override Task<WebApplication> AddModuleMiddleware(WebApplication app) => Task.FromResult(app);
}

