
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Projects;
using SaasFactory.Shared.Config;
using SaasFactory.Features.Authentication;
using SaasFactory.AppHost;

var pgUsername = "postgres";
var pgPassword = "postgres";

var builder = DistributedApplication.CreateBuilder(args);
using var loggerFactory = LoggerFactory.Create(logging => logging.AddConsole());
var logger = loggerFactory.CreateLogger("Aspire logger");

builder.Services.AddHealthChecks();

builder.Configuration
    .UseSharedConfiguration()
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

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

string authClientSecret = Environment.GetEnvironmentVariable("AUTH_CLIENT_SECRET") ?? new Crypto().GenerateText(128);

var keycloak = builder.AddKeycloakAuthServer(logger, authClientSecret);

var webApiProjectBuilder = builder.AddProject<SaasFactory_WebApi>("WebApi")
    .WithReference(keycloak);

eventsDb.OnConnectionStringAvailable(async (db, evt, ct) =>
{
    var connStr = await postgres.Resource.ConnectionStringExpression.GetValueAsync(ct);

    webApiProjectBuilder
        .WithEnvironment("EVENTS_DB_CONNECTION", connStr)
        .WithReference(postgres);
});

webApiProjectBuilder
    .WaitFor(postgres);


builder.AddNpmApp("Storybook", workingDirectory: @"..\..\..\Frontend\", scriptName: "storybook")
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(env: "PORT", port: 4300, targetPort: 6006)
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.AddNpmApp("QRCodeSite", workingDirectory: @"..\..\..\Frontend\", scriptName: "dev:web")
    .WithHttpEndpoint(env: "PORT", port: 4200, targetPort: 5173)
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

await builder.Build().RunAsync();
