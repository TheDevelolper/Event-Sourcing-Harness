namespace SaasFactory.WebApi.Extensions;

public static class BuilderExtensions
{
    public static IHostApplicationBuilder AddLogging(this IHostApplicationBuilder builder)
    {
        builder.Logging.AddConsole();
        builder.Logging.SetMinimumLevel(LogLevel.Debug);
        return builder;
    }
    

}