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

var featureToggles = new FeatureToggles();
configuration.GetSection("toggles").Bind(featureToggles);

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

await builder.Build().RunAsync();
