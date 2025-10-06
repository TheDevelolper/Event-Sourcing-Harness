using SaasFactory.Modules.Common;

namespace SaasFactory.WebApi.Extensions;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddLoggingConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.SetMinimumLevel(LogLevel.Debug);

        return builder;
    }

    public static async Task<IHostApplicationBuilder> AddFeatureModules(this IHostApplicationBuilder builder, List<IFeatureModule> featureModules)
    { 
        foreach (var featureModule in featureModules)
        {
            await featureModule.AddModule(builder);
        }
        return  builder;
    }

}