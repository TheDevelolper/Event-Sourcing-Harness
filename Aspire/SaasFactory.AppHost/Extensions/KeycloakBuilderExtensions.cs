using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SaasFactory.AppHost.Utils;

namespace SaasFactory.AppHost.Extensions;

public static class KeycloakBuilderExtensions
{
       public static IResourceBuilder<KeycloakResource> AddKeycloakAuthServer(
           this IDistributedApplicationBuilder builder, ILogger logger, string authClientSecret)
    {
        var keycloak =
            builder.AddKeycloak("keycloak", 8080)
                .WithDataVolume()
                .WithHttpsEndpoint(8443, 8443);

        // Security > Client Secret
        keycloak.WithEnvironment("KEYCLOAK_CLIENT_SECRET", authClientSecret);

        // Security > Certificate
        var basePath = AppContext.BaseDirectory;
        var secretsPath = Path.Combine(basePath, "config", ".secrets");
        var (crtPath, keyPath) = AddDevHttpCert(builder, secretsPath, logger);
        
        keycloak.WithBindMount(crtPath, "/etc/x509/https/tls.crt");
        keycloak.WithBindMount(keyPath, "/etc/x509/https/tls.key");

        // File Mounts 
        if (builder.Environment.IsDevelopment())
        {
            var fileMountsPath = Path.Combine(basePath, "Mounts");
            var keycloakConfigMountPath = Path.Combine(fileMountsPath, "Keycloak", "data", "import");
            var keycloakThemesMountPath = Path.Combine(fileMountsPath, "Keycloak", "themes", "menuota");
            keycloak
                .WithBindMount(keycloakConfigMountPath, "/opt/keycloak/data/import")
                .WithBindMount(keycloakThemesMountPath, "/opt/keycloak/themes/menuota")
                .WithEnvironment("KEYCLOAK_ADMIN", "admin")
                .WithEnvironment("KEYCLOAK_ADMIN_PASSWORD", "admin");

            logger.LogInformation("Keycloak realm import file found and mounted.");
        }

        // Configuration
        keycloak.WithEnvironment("QUARKUS_HTTP_HTTP2", "false")
            .WithArgs(cb =>
            {
                cb.Args.Add("--https-certificate-file=/etc/x509/https/tls.crt");
                cb.Args.Add("--https-certificate-file=/etc/x509/https/tls.crt");
                cb.Args.Add("--https-certificate-key-file=/etc/x509/https/tls.key");
                cb.Args.Add("--https-port=8443");
                cb.Args.Add("--hostname-strict=false");
            }).WithEntrypoint("/opt/keycloak/bin/kc.sh");

        return keycloak;
    }

    private static (string, string) AddDevHttpCert(IDistributedApplicationBuilder builder, string secretsPath, ILogger logger)
    {
        const string crtFileName = "localhost.crt";
        const string keyFileName = "localhost-key.pem";

        Directory.CreateDirectory(secretsPath);
        var crtPath = Path.Combine(secretsPath, crtFileName);
        var keyPath = Path.Combine(secretsPath, keyFileName);

        if (builder.Environment.IsDevelopment())
        {
            CertificateExporter.ExportToCrtAndPem(secretsPath, crtFileName, keyFileName, logger);
        }

        return (crtPath, keyPath);
    }
}