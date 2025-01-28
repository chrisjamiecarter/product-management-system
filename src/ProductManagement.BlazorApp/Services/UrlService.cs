using Microsoft.AspNetCore.Components;
using ProductManagement.BlazorApp.Interfaces;

namespace ProductManagement.BlazorApp.Services;

public class UrlService : IUrlService
{
    private readonly NavigationManager _navigationManager;

    public UrlService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    public string GetConfirmEmailUrl()
    {
        return _navigationManager.ToAbsoluteUri("Account/ConfirmEmail").AbsoluteUri;
    }
}
