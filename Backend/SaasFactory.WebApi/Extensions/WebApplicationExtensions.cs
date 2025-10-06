using SaasFactory.Modules.Common;

namespace SaasFactory.WebApi.Extensions;

public static class WebApplicationExtensions
{
    public static async Task<WebApplication> AddFeatureMiddleware(
        this WebApplication app, IEnumerable<IFeatureModule> middlewareModules)
    { 
        foreach (var module in middlewareModules)
        {
            await module.AddModuleMiddleware(app);
        }
        return app;
    }
}