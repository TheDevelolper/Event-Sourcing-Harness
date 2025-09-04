using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<BankStream>("BankStreamAPI");

builder.AddContainer("rabbitmq", "rabbitmq", "3-management")
    .WithEndpoint( 5672, 5672, name: "AMPQ")
    .WithEndpoint(15672, 15672, name: "ManagementUI")
    .WithContainerName("RabbitMQ");

builder.Build().Run();
