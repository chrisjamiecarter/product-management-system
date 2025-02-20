namespace ProductManagement.Application.Models;

/// <summary>
/// Represents an external login for the application.
/// </summary>
public sealed record ExternalLoginDto(string Email,
                                      string Provider,
                                      string ProviderKey,
                                      string? ProviderDisplayName);
