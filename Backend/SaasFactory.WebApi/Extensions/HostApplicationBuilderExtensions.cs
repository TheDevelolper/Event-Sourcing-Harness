namespace SaasFactory.WebApi.Extensions;

public static class HostApplicationBuilderExtensions
{

    public static IHostApplicationBuilder AddControllers(this IHostApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        return builder;
    }
    

}