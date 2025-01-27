using System.ComponentModel.DataAnnotations;

namespace ProductManagement.BlazorApp.Components.Account.Models;

internal sealed class ResendEmailConfirmationInputModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
