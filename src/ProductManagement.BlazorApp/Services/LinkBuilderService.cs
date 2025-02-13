using Microsoft.AspNetCore.Components;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Application.Models;

namespace ProductManagement.BlazorApp.Services;

/// <summary>
/// Provides the service for link building operations.
/// </summary>
internal class LinkBuilderService : ILinkBuilderService
{
    private readonly NavigationManager _navigationManager;

    public LinkBuilderService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    public Task<string> BuildChangeEmailConfirmationLinkAsync(string userId, string email, AuthToken token, CancellationToken cancellationToken = default)
    {
        var url = _navigationManager.ToAbsoluteUri("Account/ConfirmEmailChange").AbsoluteUri;

        var builder = new UriBuilder(url)
        {
            Query = $"userId={userId}&email={email}&code={token.Code}"
        };

        return Task.FromResult(builder.ToString());
    }

    public Task<string> BuildEmailConfirmationLinkAsync(string userId, AuthToken token, CancellationToken cancellationToken = default)
    {
        var url = _navigationManager.ToAbsoluteUri("Account/ConfirmEmail").AbsoluteUri;

        var builder = new UriBuilder(url)
        {
            Query = $"userId={userId}&code={token.Code}"
        };

        return Task.FromResult(builder.ToString());
    }

    public Task<string> BuildPasswordResetLinkAsync(AuthToken token, CancellationToken cancellationToken = default)
    {
        var url = _navigationManager.ToAbsoluteUri("Account/ResetPassword").AbsoluteUri;

        var builder = new UriBuilder(url)
        {
            Query = $"code={token.Code}"
        };

        return Task.FromResult(builder.ToString());
    }
}
