using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.Interfaces.Infrastructure;

namespace Microsoft.AspNetCore.Routing;

/// <summary>
/// Extensions for endpoints required by the Identity Razor components.
/// </summary>
internal static class IdentityComponentsEndpointRouteBuilderExtensions
{
    public static IEndpointConventionBuilder MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var accountGroup = endpoints.MapGroup("/Account");

        accountGroup.MapPost("/Logout", async (
            ClaimsPrincipal user,
            [FromServices] IAuthService authService,
            [FromForm] string returnUrl) =>
        {
            await authService.SignOutAsync();
            return TypedResults.LocalRedirect($"~/{returnUrl}");
        });

        return accountGroup;
    }
}
