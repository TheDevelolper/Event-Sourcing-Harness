using JasperFx;
using JasperFx.Events.Daemon;
using Marten;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SaasFactory.EventSourcing.Contracts;

namespace SaasFactory.EventSourcing.Marten;

public static class BuilderExtensions
{
    public static IServiceCollection AddEventStore(
        this IServiceCollection services,
        WebApplicationBuilder builder,
        string connectionString,
        bool enableLogging = false
        )
    {
        services.AddScoped<IEventStore, MartenStore>();

        services.AddMarten(options =>
        {
            options.Connection(connectionString);
            options.Events.AddEventType(typeof(IEvent));
            options.UseSystemTextJsonForSerialization();
            options.AutoCreateSchemaObjects = AutoCreate.All;
            options.DisableNpgsqlLogging = !enableLogging;
        }).AddAsyncDaemon(DaemonMode.Solo);

        return services;
    }
}