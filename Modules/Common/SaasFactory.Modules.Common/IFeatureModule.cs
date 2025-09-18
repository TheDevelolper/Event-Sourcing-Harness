using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace SaasFactory.Modules.Common;

public interface IFeatureModule
{
    Task<IHostApplicationBuilder> AddModule(IHostApplicationBuilder builder);
    Task<WebApplication> AddModuleMiddleware(WebApplication app);
    void RegisterMessageConsumers(IBusRegistrationConfigurator config);
}
