using System.ComponentModel.DataAnnotations;

namespace ProductManagement.BlazorApp.Components.Account.Models;

/// <summary>
/// Represents the input model for changing an email.
/// </summary>
internal sealed class ChangeEmailInputModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "New email")]
    public string? UpdatedEmail { get; set; }
}
