using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Projects;
using SaasFactory.Shared.Config;

var builder = DistributedApplication.CreateBuilder(args);

// Explicitly tell Aspire to load the shared library
var healthChecks = builder.Services.AddHealthChecks();
var configuration = builder.Configuration
    .UseSharedConfiguration()
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

var featureToggles = new FeatureToggles();
configuration.GetSection("toggles").Bind(featureToggles);

var project = builder.AddProject<SaasFactory_WebApi>("WebApi");

if (featureToggles.UseRabbitMq)
{
    builder.AddContainer("rabbitmq", "rabbitmq", "3-management")
        .WithEndpoint( 5672, 5672, name: "AMPQ")
        .WithEndpoint(15672, 15672, name: "ManagementUI")
        .WithContainerName("RabbitMQ");
}

builder.Build().Run();
