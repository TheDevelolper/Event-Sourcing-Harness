using Microsoft.Extensions.FileProviders;
using SaasFactory.Modules.Common;
using Serilog.Core;

namespace SaasFactory.Authentication;

public class AuthenticationModule(string clientSecret, Logger logger): FeatureModule<AuthenticationOptions>(logger)
{
    protected override string ModuleName => nameof(AuthenticationModule);

    public override Task<IHostApplicationBuilder> AddModule(IHostApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(clientSecret, logger);
        return Task.FromResult(builder);
    }

    public override Task<WebApplication> AddModuleMiddleware(WebApplication app)
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

public class AuthenticationOptions: ModuleOptionsBase
{
    
}