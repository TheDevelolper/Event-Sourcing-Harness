using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Modules.Examples.Restaurant.Menu.Features.Menu;
    
public static class CreateMenuItem
{
    public static void AddGetMenuItemByIdEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/menu/by-id/{menuId}", async (int menuId) =>
        {
            var result = $"You requested menu id '{menuId}'";
            return Results.Ok(result);
        });
    }
}
