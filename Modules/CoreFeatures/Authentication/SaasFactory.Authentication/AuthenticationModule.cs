using Microsoft.Extensions.FileProviders;
using SaasFactory.Modules.Common;
using Serilog.Core;

namespace SaasFactory.Authentication;

public static class AuthenticationConfig
{
    public const string SectionName = "Modules:Authentication";
}

public class AuthenticationModule(string clientSecret, Logger logger) : IFeatureModule
{
    public Task<IHostApplicationBuilder> AddModule(IHostApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(clientSecret, logger);
        return Task.FromResult(builder);
    }

    public Task<WebApplication> AddModuleMiddleware(WebApplication app)
    {
        var assembly = typeof(AuthenticationModule).Assembly;

        // IMPORTANT: this must match the root namespace + folder name
        const string baseNamespace = "SaasFactory.Authentication.Web";
        var fileProvider = new EmbeddedFileProvider(assembly, baseNamespace);
        
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = fileProvider,
            RequestPath = "/Authentication"
        });

        return Task.FromResult(app);
    }
}