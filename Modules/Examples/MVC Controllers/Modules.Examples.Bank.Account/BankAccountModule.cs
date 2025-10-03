using FeatureHubSDK;
using JasperFx.Events.Projections;
using Marten;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Modules.Examples.Bank.Account.Events;
using Modules.Examples.Bank.Account.Projections;
using SaasFactory.Modules.Common;

namespace Modules.Examples.Bank.Account;

public class BankAccountModule(IClientContext featureHubCtx): IFeatureModule
{
    private static bool _isEnabled;
    public static bool IsEnabled
    {
        get => _isEnabled;
        private set
        {
            _isEnabled = value;
            var enabledStatus = IsEnabled  ? "Enabled" : "Disabled";
            Console.WriteLine($"Bank Account Module {enabledStatus}");
            OnIsEnabledChanged?.Invoke(null, value);
        }
    }
    public static event EventHandler<bool>? OnIsEnabledChanged;

    public Task<IHostApplicationBuilder> AddModule(IHostApplicationBuilder builder)
    {
        var moduleFeature = featureHubCtx[nameof(BankAccountModule)];
        IsEnabled = moduleFeature.BooleanValue ?? false;
        
        moduleFeature.FeatureUpdateHandler += (sender, feature) =>
        {
            var featureEnabled = feature.BooleanValue ?? false;
            IsEnabled = featureEnabled;
        };
        
        builder.Services.ConfigureMarten(options =>
        {
            options.Projections.Add<AccountStateProjection>(ProjectionLifecycle.Async);
            options.Events.AddEventType(typeof(DepositCompletedEvent));
        });
    
        return Task.FromResult(builder);
    }

    public Task<WebApplication> AddModuleMiddleware(WebApplication app) => Task.FromResult(app);

}