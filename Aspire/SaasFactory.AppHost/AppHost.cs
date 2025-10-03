using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Projects;
using SaasFactory.Shared.Config;
using SaasFactory.Features.Authentication;
using SaasFactory.AppHost;

var pgUsername = "postgres";
var pgPassword =  "postgres";
var featureHubPort = 4242;

var builder = DistributedApplication.CreateBuilder(args);
using var loggerFactory = LoggerFactory.Create(logging => logging.AddConsole());
var logger = loggerFactory.CreateLogger("Aspire logger");

builder.Services.AddHealthChecks();

builder.Configuration
    .UseSharedConfiguration()
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

var rabbitMq = builder.AddContainer("rabbitmq", "rabbitmq", "3-management")
     .WithEndpoint(5672, 5672, name: "AMPQ")
     .WithEndpoint(15672, 15672, name: "ManagementUI", scheme: "http")
     .WithContainerName("RabbitMQ")
     .WithHttpHealthCheck("/api/index.html", endpointName: "ManagementUI");

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
var featureHubDb = postgres.AddDatabase("featurehub");

// ======================
// Create containers
// ======================

string authClientSecret = Environment.GetEnvironmentVariable("AUTH_CLIENT_SECRET") ?? new Crypto().GenerateText(128);

var keycloak = builder.AddKeycloakAuthServer(logger, authClientSecret);

var featurehub = builder.AddContainer("featurehub-server", "featurehub/party-server")
    .WithEnvironment("FEATUREHUB_HTTP_PORT", "8085")
    .WithEnvironment("FEATUREHUB_JWT_SECRET", "supersecretkey") //TODO(PROD): change in production
    .WithEnvironment("FEATUREHUB_EDGE_URL", $"http://featurehub-server:{featureHubPort.ToString()}")
    .WithEnvironment("FEATUREHUB_API_KEY", "")
    .WithEndpoint(port: featureHubPort, targetPort: 8085, scheme: "http") // default HTTP port
    
    // Database connection
    .WithEnvironment("DB_URL",
        $"jdbc:postgresql://{postgres.Resource.Name}:5432/{featureHubDb.Resource.DatabaseName}")
    .WithEnvironment("DB_USERNAME", pgUsername)
    .WithEnvironment("DB_PASSWORD", pgPassword)
    .WithReference(postgres)
    .WaitFor(postgres);
    
var featureHubUrl = new Uri($"http://{featurehub.Resource.Name}:{featureHubPort.ToString()}");

var webApiProjectBuilder = builder.AddProject<SaasFactory_WebApi>("WebApi") 
    .WithEnvironment("FEATUREHUB_API_KEY", "3ba59a66-b00d-459d-af2a-4b46cfcfa66d/x9a7Q8BZ12GnKLQttv14UcpAfEt9ceI6DmbJotS2")
    .WithEnvironment("FEATUREHUB_EDGE_URL", $"http://localhost:{featureHubPort.ToString()}")
    .WithReference("featurehub", featureHubUrl)
    .WithReference(keycloak);

eventsDb.OnConnectionStringAvailable(async (db, evt, ct) =>
{
    var connStr = await postgres.Resource.ConnectionStringExpression.GetValueAsync(ct);
    
    webApiProjectBuilder
        .WithEnvironment("EVENTS_DB_CONNECTION", connStr)
        .WithReference(postgres);
});

webApiProjectBuilder
    .WaitFor(postgres)
    .WaitFor(rabbitMq);

await builder.Build().RunAsync();
 