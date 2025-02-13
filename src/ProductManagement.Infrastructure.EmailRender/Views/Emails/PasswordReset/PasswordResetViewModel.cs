namespace ProductManagement.Infrastructure.EmailRender.Views.Emails.PasswordReset;

/// <summary>
/// Represents the model for a password reset view.
/// </summary>
/// <param name="PasswordResetLink">The url for resetting a password.</param>
public record PasswordResetViewModel(string PasswordResetLink);
