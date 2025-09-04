using BankStream.Consumers;
using BankStream.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddDbContext<EventDbContext>(options => options.UseSqlite("Data Source=events.db"));

// builder.AddDefaultHealthChecks();
builder.AddServiceDefaults();

builder
    .Services
    .AddMassTransit(x =>
    {
        x.AddConsumer<DepositConsumer>();
        x.AddConsumer<WithdrawalConsumer>();
        x.UsingRabbitMq(
            (context, cfg) =>
            {
                cfg.Host("rabbitmq://localhost");
                cfg.ReceiveEndpoint(
                    "event-queue",
                    e =>
                    {
                        e.SetQueueArgument("x-message-ttl", 500000);
                        e.ConfigureConsumer<DepositConsumer>(context);
                        e.ConfigureConsumer<WithdrawalConsumer>(context);
                    }
                );
            }
        );
    });

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();