using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Projects;
using SaasFactory.Shared.Config;

var builder = DistributedApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

var configuration = builder.Configuration
    .UseSharedConfiguration()
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

var featureToggles = FeatureToggles.FromConfig(configuration);

var webApiProject = builder.AddProject<SaasFactory_WebApi>("WebApi");

if (featureToggles.UseRabbitMq)
{
    var rabbitMq = builder.AddContainer("rabbitmq", "rabbitmq", "3-management")
        .WithEndpoint(5672, 5672, name: "AMPQ")
        .WithEndpoint(15672, 15672, name: "ManagementUI", scheme: "http")
        .WithContainerName("RabbitMQ")
        .WithHttpHealthCheck("/api/index.html", endpointName: "ManagementUI");
    webApiProject.WaitFor(rabbitMq);
}

if (featureToggles.UseEventStore == SupportedEventStores.EventStoreDb)
{
    var eventStore = builder.AddContainer("eventstore", "eventstore/eventstore", "latest")
    .WithEnvironment("EVENTSTORE_CLUSTER_SIZE", "1")
    .WithEnvironment("EVENTSTORE_RUN_PROJECTIONS", "All")
    .WithEnvironment("EVENTSTORE_START_STANDARD_PROJECTIONS", "true")
    .WithEnvironment("EVENTSTORE_INSECURE", "true")
    .WithEndpoint(port: 2113, targetPort: 2113, scheme: "http") // UI & HTTP API
    .WithEndpoint(port: 1113, targetPort: 1113) // TCP
    .WithHttpHealthCheck("/web/index.html#/dashboard", endpointName: "http");

    webApiProject.WaitFor(eventStore);
}

await builder.Build().RunAsync();
