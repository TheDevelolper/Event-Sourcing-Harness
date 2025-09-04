using Microsoft.EntityFrameworkCore;
using SaasFactory.Shared.Config;
using SaasFactory.WebApi.Controllers;
using SaasFactory.WebApi.Data;
using SaasFactory.WebApi.Data.EntityFramework;
using SaasFactory.WebApi.Data.EventStoreDb;

namespace SaasFactory.WebApi.Extensions;

public static class BuilderExtensions
{
    public static IHostApplicationBuilder AddEventStore(this IHostApplicationBuilder builder, SupportedEventStores store)
    {
        switch (store)
        {
            case SupportedEventStores.EntityFramework:
                builder.Services.AddScoped<IEventStore, EntityFrameworkEventStore>();
            break;
            case SupportedEventStores.EventStoreDb:
                builder.Services.AddScoped<IEventStore, EventStoreDb>();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return builder;
    }
}