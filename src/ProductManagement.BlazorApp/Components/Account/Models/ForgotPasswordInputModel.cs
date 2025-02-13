using System.ComponentModel.DataAnnotations;

namespace ProductManagement.BlazorApp.Components.Account.Models;

/// <summary>
/// Represents the input model for requesting a password reset link.
/// </summary>
public sealed class ForgotPasswordInputModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
