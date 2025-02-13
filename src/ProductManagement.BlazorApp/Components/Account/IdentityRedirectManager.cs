using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using ProductManagement.BlazorApp.Components.Shared.Models;

namespace ProductManagement.BlazorApp.Components.Account;

/// <summary>
/// Manages redirection logic for identity-related actions, including navigation and status message handling.
/// </summary>
/// <param name="navigationManager">The navigation manager object.</param>
internal sealed class IdentityRedirectManager(NavigationManager navigationManager)
{
    public const string StatusMessageCookieName = "Identity.StatusMessage";
    public const string StatusLevelCookieName = "Identity.StatusLevel";

    private static readonly CookieBuilder StatusCookieBuilder = new()
    {
        SameSite = SameSiteMode.Strict,
        HttpOnly = true,
        IsEssential = true,
        MaxAge = TimeSpan.FromSeconds(5),
    };

    [DoesNotReturn]
    public void RedirectTo(string? uri)
    {
        uri ??= "";

        // Prevent open redirects.
        if (!Uri.IsWellFormedUriString(uri, UriKind.Relative))
        {
            uri = navigationManager.ToBaseRelativePath(uri);
        }

        // During static rendering, NavigateTo throws a NavigationException which is handled by the framework as a redirect.
        // So as long as this is called from a statically rendered Identity component, the InvalidOperationException is never thrown.
        navigationManager.NavigateTo(uri);
        throw new InvalidOperationException($"{nameof(IdentityRedirectManager)} can only be used during static rendering.");
    }

    [DoesNotReturn]
    public void RedirectTo(string uri, Dictionary<string, object?> queryParameters)
    {
        var uriWithoutQuery = navigationManager.ToAbsoluteUri(uri).GetLeftPart(UriPartial.Path);
        var newUri = navigationManager.GetUriWithQueryParameters(uriWithoutQuery, queryParameters);
        RedirectTo(newUri);
    }

    [DoesNotReturn]
    public void RedirectToWithStatus(string uri, StatusModel status, HttpContext context)
    {
        var cookieOptions = StatusCookieBuilder.Build(context);
        context.Response.Cookies.Append(StatusMessageCookieName, status.Message ?? string.Empty, cookieOptions);
        context.Response.Cookies.Append(StatusLevelCookieName, status.Level.ToString() ?? string.Empty, cookieOptions);
        RedirectTo(uri);
    }

    private string CurrentPath => navigationManager.ToAbsoluteUri(navigationManager.Uri).GetLeftPart(UriPartial.Path);

    [DoesNotReturn]
    public void RedirectToCurrentPage() => RedirectTo(CurrentPath);

    [DoesNotReturn]
    public void RedirectToCurrentPageWithStatus(StatusModel status, HttpContext context)
        => RedirectToWithStatus(CurrentPath, status, context);
}
