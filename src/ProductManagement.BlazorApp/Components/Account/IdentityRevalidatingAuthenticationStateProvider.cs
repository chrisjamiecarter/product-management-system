using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using ProductManagement.Application.Interfaces.Infrastructure;

namespace ProductManagement.BlazorApp.Components.Account;

/// <summary>
/// This is a server-side <see cref="AuthenticationStateProvider"/> that revalidates the security stamp for the connected user every 30 minutes an interactive circuit is connected.
/// </summary>
/// <param name="loggerFactory">The logger factory object.</param>
/// <param name="scopeFactory">The scope factory object.</param>
internal sealed class IdentityRevalidatingAuthenticationStateProvider(
        ILoggerFactory loggerFactory,
        IServiceScopeFactory scopeFactory)
    : RevalidatingServerAuthenticationStateProvider(loggerFactory)
{
    protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

    protected override async Task<bool> ValidateAuthenticationStateAsync(
        AuthenticationState authenticationState, CancellationToken cancellationToken)
    {
        // Get the auth service from a new scope to ensure it fetches fresh data.
        await using var scope = scopeFactory.CreateAsyncScope();
        var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

        var validationResult = await authService.ValidateSecurityStampAsync(authenticationState.User, cancellationToken);
        return validationResult.IsSuccess;
    }
}
