namespace ProductManagement.Application.Constants;

/// <summary>
/// Defines the application password requirements.
/// </summary>
public static class PasswordOptions
{
    public const int RequiredLength = 8;
    public const bool RequireDigit = true;
    public const bool RequireLowercase = true;
    public const bool RequireUppercase = true;
    public const bool RequireNonAlphanumeric = true;
}
