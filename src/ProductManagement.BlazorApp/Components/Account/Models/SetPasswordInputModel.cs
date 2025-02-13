using System.ComponentModel.DataAnnotations;
using ProductManagement.Application.Constants;

namespace ProductManagement.BlazorApp.Components.Account.Models;

/// <summary>
/// Represents the input model for setting a password.
/// </summary>
internal sealed class SetPasswordInputModel
{
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = PasswordOptions.RequiredLength)]
    [DataType(DataType.Password)]
    [Display(Name = "New password")]
    public string UpdatedPassword { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Confirm new password")]
    [Compare("UpdatedPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
