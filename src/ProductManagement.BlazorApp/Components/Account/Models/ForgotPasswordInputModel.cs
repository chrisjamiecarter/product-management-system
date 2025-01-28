using System.ComponentModel.DataAnnotations;

namespace ProductManagement.BlazorApp.Components.Account.Models;

public sealed class ForgotPasswordInputModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
