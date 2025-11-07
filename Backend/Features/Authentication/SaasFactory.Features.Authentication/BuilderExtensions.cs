using Ardalis.GuardClauses;
using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using SaasFactory.Features.Authentication.Utils;

namespace SaasFactory.Features.Authentication;

public static class BuilderExtensions
{
    //TODO: From Keycloak Clients > Client details > Credentials (Tab)
    //TODO(issue: #8) This is only for development, will come from settings. Just haven't got to this task yet.

    public static IResourceBuilder<KeycloakResource> AddKeycloakAuthServer(
     this IDistributedApplicationBuilder builder, ILogger logger, string authClientSecret)
    {
        if (string.IsNullOrWhiteSpace(authClientSecret))
        {
            // Fix: Log into Keycloak goto client details then credentials
            // generate a client secret and set the environment variable.
            // see code in apphost where this variable is read.
            logger.LogError(@"No auth client secret provided. Get one from keycloak and set environment var on the host.");
            
        }

        var keycloak =
            builder.AddKeycloak("keycloak", 8080)
                .WithDataVolume()
                .WithHttpsEndpoint(8443, 8443);

        keycloak
            .WithEnvironment("KEYCLOAK_CLIENT_SECRET", authClientSecret);

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
            var jarSource = Path.Combine(basePath, "Mounts", "Keycloak", "providers", "keycloak-theme-for-kc-all-other-versions.jar");

            if (File.Exists(jarSource) == false) throw new Exception("OOOOf");

            keycloak
                .WithEnvironment("KEYCLOAK_THEME_CACHE", "none")
                .WithEnvironment("KC_SPI_THEME_STATIC_MAX_AGE", "1") // fix theme cache preventing changes showing.
                .WithEnvironment("KC_BOOTSTRAP_ADMIN_USERNAME", "admin")
                .WithEnvironment("KC_BOOTSTRAP_ADMIN_PASSWORD", "admin")
                .WithBindMount(keycloakConfigMountPath, "/opt/keycloak/data/import")
                .WithBindMount(jarSource, "/opt/keycloak/providers/keycloak-theme.jar");
        }

        // Configuration
        keycloak.WithEnvironment("QUARKUS_HTTP_HTTP2", "false")
            .WithArgs(cb =>
            {
                cb.Args.Add("start");
                cb.Args.Add("--https-certificate-file=/etc/x509/https/tls.crt");
                cb.Args.Add("--https-certificate-file=/etc/x509/https/tls.crt");
                cb.Args.Add("--https-certificate-key-file=/etc/x509/https/tls.key");
                cb.Args.Add("--https-port=8443");
                cb.Args.Add("--health-enabled=true");
                cb.Args.Add("--hostname-strict=false");
            }).WithEntrypoint("/opt/keycloak/bin/kc.sh");

        return keycloak;
    }

    private static (string, string) AddDevHttpCert(IDistributedApplicationBuilder builder,
        string secretsPath, ILogger logger)
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

    public static IHostApplicationBuilder AddAuthentication(
        this IHostApplicationBuilder builder, string authClientSecret)
    {

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder =>
                {
                    builder.WithOrigins("http://localhost:8080")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithExposedHeaders("Access-Control-Allow-Origin",
                            "Access-Control-Allow-Credentials"); // Add exposed headers if needed
                });
        });

        builder.Services.AddAuthorization();
        // Configure the authentication services with OpenID Connect
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Use Cookies as the default scheme
                options.DefaultChallengeScheme =
                    OpenIdConnectDefaults.AuthenticationScheme; // OpenID Connect as the challenge scheme
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensure secure cookie
                options.Cookie.SameSite = SameSiteMode.None; // Allow cross-origin
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = "https://localhost:8443/realms/Menuota"; // Keycloak server URL
                options.ClientId = "menu-management";
                options.ClientSecret = authClientSecret; //TODO: From Keycloak Clients > Client details > Credentials (Tab)
                options.ResponseType = "code";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.UsePkce = true; // keep PKCE enabled
                options.PushedAuthorizationBehavior = PushedAuthorizationBehavior.Disable;
                options.Events = new OpenIdConnectEvents
                {
                    OnRedirectToIdentityProvider = ctx =>
                    {
                        Console.WriteLine("Redirect URI being sent to Keycloak:");
                        Console.WriteLine(ctx.ProtocolMessage.RedirectUri ?? "<NULL>");
                        return Task.CompletedTask;
                    }
                };

                // Disable HTTPS metadata requirement for development
                // TODO(PROD): Enable In Production
                options.RequireHttpsMetadata = false;

                // Handle the redirect after login
                options.CallbackPath = "/signin-oidc"; // Ensure this matches the redirect URI in Keycloak
            });

        return builder;
    }
}