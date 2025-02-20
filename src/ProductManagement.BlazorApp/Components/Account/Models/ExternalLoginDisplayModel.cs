using ProductManagement.Application.Models;

namespace ProductManagement.BlazorApp.Components.Account.Models;

/// <summary>
/// Represents the display model for an external login for a user.
/// </summary>
public sealed class ExternalLoginDisplayModel
{
    public ExternalLoginDisplayModel(ExternalLoginDto externalLoginDto)
    {
        Email = externalLoginDto.Email;
        Provider = externalLoginDto.Provider;
        ProviderKey = externalLoginDto.ProviderKey;
        ProviderDisplayName = externalLoginDto.ProviderDisplayName;
    }

    public string Email { get; set; } = string.Empty;

    public string Provider { get; set; } = string.Empty;

    public string ProviderKey { get; set; } = string.Empty;

    public string? ProviderDisplayName { get; set; } = string.Empty;

    public string DisplayName => ProviderDisplayName ?? Provider;

    public bool IsLinked => !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(ProviderKey);
}
