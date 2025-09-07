using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Modules.Examples.Restaurant.Menu.Features.Menu;

namespace Modules.Examples.Restaurant.Menu;

public static class RestaurantMenuModuleExample
{
    public static IHostApplicationBuilder AddRestaurantMenuModuleExample(this IHostApplicationBuilder app)
    {
        // registrations for other services go here...
        return app;
    }

    public static void UseRestaurantMenuModuleExample(this WebApplication app)
    {
        app.AddGetMenuItemByIdEndpoint();
    }
}


