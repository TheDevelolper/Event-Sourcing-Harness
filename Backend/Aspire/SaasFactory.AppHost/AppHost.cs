using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Projects;
using SaasFactory.Authentication;
using SaasFactory.Shared.Config;
using Serilog;
using Serilog.Formatting.Display;
using Serilog.Sinks.Grafana.Loki;

const string pgUsername = "postgres";
const string pgPassword = "postgres";

var builder = DistributedApplication.CreateBuilder(args); 


var textFormatter = new MessageTemplateTextFormatter("{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}", null);
const string lokiUrl = "http://localhost:3100";

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.GrafanaLoki(
        lokiUrl,
        textFormatter: textFormatter, // ✅ render as plain text
        labels:
        [
            new LokiLabel { Key = "app", Value = "saasfactory" },
            new LokiLabel { Key = "env", Value = "dev" }
        ])
    .CreateLogger();


builder.Services.AddHealthChecks();

builder.Configuration
    .UseSharedConfiguration()
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

// ======================
// Logging with Loki and Graphana
// ======================

// Loki container
var loki = builder.AddContainer("loki", "grafana/loki:3.1.1")
    .WithVolume("loki_data", "/var/loki") // ✅ creates a Docker volume named loki_data
    .WithEndpoint(3100, targetPort: 3100, scheme: "http")
    .WithArgs("--config.file=/etc/loki/local-config.yaml");

// Grafana container
var grafana = builder.AddContainer("grafana", "grafana/grafana:11.1.0")
    .WithEndpoint(3000, targetPort: 3000, scheme: "http")
    .WithVolume("grafana_data", "/var/lib/grafana") // persist dashboards etc.
    .WithEnvironment("GF_SECURITY_ADMIN_USER", "admin")
    .WithEnvironment("GF_SECURITY_ADMIN_PASSWORD", "admin")
    .WithEnvironment("GF_AUTH_ANONYMOUS_ENABLED", "true")
    .WithEnvironment("GF_AUTH_ANONYMOUS_ORG_ROLE", "Admin")
    .WithEnvironment("GF_DATASOURCES_DEFAULT_TYPE", "loki")
    .WithEnvironment("GF_DATASOURCES_DEFAULT_URL", "http://loki:3100")
    .WithExternalHttpEndpoints();

// ======================
// Create postgres database Server
// ======================
var postgres = builder.AddPostgres("postgres",
        userName: builder.AddParameter("username", pgUsername, secret: true),
        password: builder.AddParameter("password", pgPassword, secret: true))
    .WithDataVolume() // persists data between runs
    .WithPgAdmin(pgAdminBuilder => pgAdminBuilder.WithHostPort(8081))
    .WithEndpoint(port: 54320, targetPort: 5432, scheme: "tcp", name: "postgres-tcp");

// ======================
// Create databases
// ======================
var eventsDb = postgres.AddDatabase("events");

// ======================
// Create containers
// ======================

var clientSecretEnvVar = builder.Configuration["Authentication:ClientSecretEnvironmentVar"] ?? string.Empty;

string devClientSecret = "**********";
var authClientSecret = Environment.GetEnvironmentVariable(clientSecretEnvVar) ?? devClientSecret; 
var keycloak = builder.AddKeycloakAuthServer(logger, authClientSecret);

var webApiResourceBuilder = builder.AddProject<SaasFactory_WebApi>("WebApi")
    .WithReference(keycloak)
    .WithEnvironment(clientSecretEnvVar, authClientSecret)
    .WithEnvironment("LOKI_URL", lokiUrl);
    
eventsDb.OnConnectionStringAvailable(async (db, evt, ct) =>
{
    var connStr = await postgres.Resource.ConnectionStringExpression.GetValueAsync(ct);

    webApiResourceBuilder
        .WithEnvironment("EVENTS_DB_CONNECTION", connStr)
        .WithReference(postgres);
});

builder.AddExecutable("Documentation-PDF", "docfx", "../../../DocFx")
    .WithArgs("pdf");

builder.AddExecutable("Documentation-Site", "docfx", "../../../DocFx")
    .WithArgs("build", "--serve", "-p", "4400")
    .WithExternalHttpEndpoints();

webApiResourceBuilder
    .WaitFor(loki)
    .WaitFor(postgres);


builder.AddNpmApp("Storybook", workingDirectory: @"..\..\..\Frontend\", scriptName: "aspirehost:storybook")
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(env: "PORT", port: 4300, targetPort: 6006)
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.AddNpmApp("Website", workingDirectory: @"..\..\..\Frontend\", scriptName: "aspirehost:web")
    .WithHttpEndpoint(env: "PORT", port: 4200, targetPort: 5173)
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

await builder.Build().RunAsync();

