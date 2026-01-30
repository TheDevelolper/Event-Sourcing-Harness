using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog.Core;

namespace SaasFactory.Modules.Common;

public interface IFeatureModule
{
    Task<IHostApplicationBuilder> RegisterModule(IHostApplicationBuilder builder);
    Task<WebApplication> AddModuleMiddleware(WebApplication app);
}

public abstract class FeatureModule<TModuleOptions>(Logger logger): IFeatureModule
    where TModuleOptions : ModuleOptionsBase
{
    protected abstract string ModuleName { get; }
    public TModuleOptions? Options { get; private set; }
    
    public Task<IHostApplicationBuilder> RegisterModule(IHostApplicationBuilder builder)
    {
        logger.Information("Registering module {ModuleName}", ModuleName);
        var configurationSectionName = $"Modules:{ModuleName}";
        var optionsBuilder = builder.Services.AddOptions<TModuleOptions>()
            .BindConfiguration(configurationSectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Configuration.GetSection(configurationSectionName).Bind(Options);

        var result =  AddModule(builder); 
        logger.Information("Module {ModuleName} successfully registered", ModuleName);
        return result;
    }

    public abstract Task<IHostApplicationBuilder> AddModule(IHostApplicationBuilder builder);
    
    public abstract Task<WebApplication> AddModuleMiddleware(WebApplication app);
}