using ProductManagement.BlazorApp.Enums;

namespace ProductManagement.BlazorApp.Interfaces;

/// <summary>
/// Defines the service for managing toast notifications.
/// </summary>
internal interface IToastService
{
    event Action? OnHide;
    event Action<string, ToastLevel>? OnShow;

    void Dispose();
    void ShowToast(string message, ToastLevel level);
}