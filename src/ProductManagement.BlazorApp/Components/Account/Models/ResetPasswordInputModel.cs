using System.ComponentModel.DataAnnotations;
using ProductManagement.Application.Constants;

namespace ProductManagement.BlazorApp.Components.Account.Models;

/// <summary>
/// Represents the input model for resetting a password.
/// </summary>
public sealed class ResetPasswordInputModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = PasswordOptions.RequiredLength)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
