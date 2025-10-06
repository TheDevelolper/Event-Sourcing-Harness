using JasperFx;
using JasperFx.Events.Daemon;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SaasFactory.EventSourcing.Contracts;

namespace SaasFactory.EventSourcing.Marten;

public static class BuilderExtensions
{
    public static IHostApplicationBuilder AddEventStore(
        this IHostApplicationBuilder builder, 
        string connectionString,
        bool enableLogging = false
        )
    {
        builder.Services.AddScoped<IEventStore, MartenStore>();

        builder.Services.AddMarten(options =>
        {
            options.Connection(connectionString);
            options.Events.AddEventType(typeof(IEvent));
            options.UseSystemTextJsonForSerialization();
            options.AutoCreateSchemaObjects = AutoCreate.All;
            options.DisableNpgsqlLogging = !enableLogging;

        }).AddAsyncDaemon(DaemonMode.Solo);

        return builder;
    }
}