using FeatureHubSDK;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Modules.Examples.Restaurant.Menu.Features.Menu;
using SaasFactory.Modules.Common;

namespace Modules.Examples.Restaurant.Menu;

public class RestaurantMenuModule(IClientContext featureHubCtx) : IFeatureModule
{
    public Task<IHostApplicationBuilder> AddModule(IHostApplicationBuilder builder) => Task.FromResult(builder);

    public Task<WebApplication> AddModuleMiddleware(WebApplication app)
    {
        app.AddGetMenuItemByIdEndpoint();
        return Task.FromResult(app);
    }

    public void RegisterMessageConsumers(IBusRegistrationConfigurator config) { }
}
