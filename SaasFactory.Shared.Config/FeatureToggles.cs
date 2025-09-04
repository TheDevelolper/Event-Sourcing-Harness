using Microsoft.Extensions.Configuration;

namespace SaasFactory.Shared.Config;

public class FeatureToggles
{
    public bool UseRabbitMq { get; set; }
    public SupportedEventStores UseEventStore { get; set; }

    public static FeatureToggles FromConfig(IConfigurationRoot configuration)
    {
        var featureToggles = new FeatureToggles();
        configuration.GetSection("toggles").Bind(featureToggles);
        return featureToggles;
    }
}

public enum SupportedEventStores
{
    EntityFramework,
    EventStoreDb
}