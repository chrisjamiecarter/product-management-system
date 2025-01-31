using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Application.Models;

namespace ProductManagement.BlazorApp.Services;

internal class LinkBuilderService : ILinkBuilderService
{
    private readonly NavigationManager _navigationManager;

    public LinkBuilderService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    public Task<string> BuildChangeEmailConfirmationLinkAsync(string userId, string email, AuthToken token, CancellationToken cancellationToken = default)
    {
        var url = _navigationManager.ToAbsoluteUri("Account/ConfirmEmail").AbsoluteUri;

        var builder = new UriBuilder(url)
        {
            Query = $"userId={userId}&email&{email}&code={token.Code}"
        };
        var link = HtmlEncoder.Default.Encode(builder.ToString());

        return Task.FromResult(link);
    }

    public Task<string> BuildEmailConfirmationLinkAsync(string userId, AuthToken token, CancellationToken cancellationToken = default)
    {
        var url = _navigationManager.ToAbsoluteUri("Account/ConfirmEmail").AbsoluteUri;

        var builder = new UriBuilder(url)
        {
            Query = $"userId={userId}&code={token.Code}"
        };
        var link = HtmlEncoder.Default.Encode(builder.ToString());

        return Task.FromResult(link);
    }

    public Task<string> BuildPasswordResetLinkAsync(AuthToken token, CancellationToken cancellationToken = default)
    {
        var url = _navigationManager.ToAbsoluteUri("Account/ResetPassword").AbsoluteUri;

        var builder = new UriBuilder(url)
        {
            Query = $"code={token.Code}"
        };
        var link = HtmlEncoder.Default.Encode(builder.ToString());

        return Task.FromResult(link);
    }
}
