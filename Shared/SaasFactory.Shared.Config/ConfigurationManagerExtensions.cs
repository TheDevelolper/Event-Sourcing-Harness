using Microsoft.Extensions.Configuration;

namespace SaasFactory.Shared.Config;

public static class ConfigurationManagerExtensions
{
    public static IConfigurationBuilder UseSharedConfiguration(this IConfigurationBuilder builderConfiguration)
    {
        // Loads cross project shared configuration
        
        builderConfiguration
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.shared.json", optional: true); 

        return builderConfiguration;
    }
}