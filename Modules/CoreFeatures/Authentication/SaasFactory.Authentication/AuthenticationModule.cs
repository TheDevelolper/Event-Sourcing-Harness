using Microsoft.Extensions.FileProviders;
using SaasFactory.Modules.Common;
using Ardalis.GuardClauses;
using Serilog.Core;

namespace SaasFactory.Authentication;

public class AuthenticationModule(Logger logger) : FeatureModule<AuthenticationOptions>(logger)
{
    protected override string ModuleName => nameof(AuthenticationModule);

    public override Task<IHostApplicationBuilder> AddModule(IHostApplicationBuilder builder)
    {
        var clientSecretEnvVar =
            builder.Configuration["Modules:Authentication:ClientSecretEnvironmentVar"] ?? string.Empty;

        // TODO: PERHAPS WE COULD CREATE OUR OWN GUARD CLAUSES THAT RETURN RESULT OBJECTS?
        //  Then go and replace all the current guard clauses with our own.
        Guard.Against.NullOrWhiteSpace(input: clientSecretEnvVar,
            message: @"CLIENT SECRET ENVIRONMENT VARIABLE NAME IS MISSING FROM CONFIGURATION.
            follow the Authentication Client Secret Setup for reference.
            http://localhost:4400/docs/guides/authentication/authentication-client-secret-setup.html#1-add-the-configuration-setting");

        var clientSecret = Environment.GetEnvironmentVariable(clientSecretEnvVar) ?? string.Empty;
        Guard.Against.NullOrWhiteSpace(input: clientSecret,
            message: @$"CLIENT SECRET ENVIRONMENT VARIABLE IS MISSING.
            Environment Variable Name: {clientSecretEnvVar}
            follow the Authentication Client Secret Setup for reference:
            http://localhost:4400/docs/guides/authentication/authentication-client-secret-setup.html#2-set-the-environment-variable-on-the-host-system");

        logger.Information("Authentication client secret env var key found ✅");
        
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

public class AuthenticationOptions : ModuleOptionsBase
{
}