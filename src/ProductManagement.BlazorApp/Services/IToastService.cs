using ProductManagement.BlazorApp.Enums;

namespace ProductManagement.BlazorApp.Services;

/// <summary>
/// Defines the service for managing toast notifications.
/// </summary>
public interface IToastService
{
    event Action? OnHide;
    event Action<string, ToastLevel>? OnShow;

    void Dispose();
    void ShowToast(string message, ToastLevel level);
}