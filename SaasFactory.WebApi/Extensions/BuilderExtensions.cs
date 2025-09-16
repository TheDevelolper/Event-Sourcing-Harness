namespace SaasFactory.WebApi.Extensions;

public static class BuilderExtensions
{
    public static IHostApplicationBuilder AddLogging(this IHostApplicationBuilder builder, LogLevel minimumLogLevel)
    {
        builder.Logging.AddConsole();
        builder.Logging.SetMinimumLevel(minimumLogLevel);
        return builder;
    }
    public static ILogger<T> CreateBootstrapLogger<T>(this IHostApplicationBuilder builder, LogLevel minimumLogLevel)
    {
        var factory = LoggerFactory.Create(logging =>
        {
            logging.AddConsole();
            logging.SetMinimumLevel(minimumLogLevel);
        });
        return factory.CreateLogger<T>();
    }
}