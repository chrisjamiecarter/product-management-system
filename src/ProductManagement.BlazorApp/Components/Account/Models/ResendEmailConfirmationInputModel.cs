using System.ComponentModel.DataAnnotations;

namespace ProductManagement.BlazorApp.Components.Account.Models;

/// <summary>
/// Represents the input model for resending an email confirmation link.
/// </summary>
internal sealed class ResendEmailConfirmationInputModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
