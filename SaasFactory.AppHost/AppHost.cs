using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Projects;
using SaasFactory.Shared.Config;

var builder = DistributedApplication.CreateBuilder(args);

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

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume() // persists data between runs
    .WithPgAdmin(pgAdminBuilder => 
        pgAdminBuilder.WithHostPort(8081))   
    .WithEndpoint(port: 50451, targetPort: 5432, scheme: "tcp", name: "postgres-tcp")
    .AddDatabase("events");

var webApiProject = builder.AddProject<SaasFactory_WebApi>("WebApi");

postgres.OnConnectionStringAvailable<PostgresDatabaseResource>(async (db, evt, ct) =>
{
    var constr = await postgres.Resource.ConnectionStringExpression.GetValueAsync(ct);
    
    webApiProject
        .WithEnvironment("EVENTS_DB_CONNECTION", constr)
        .WithReference(postgres);
});

webApiProject
    .WaitFor(postgres)
    .WaitFor(rabbitMq);

await builder.Build().RunAsync();
