using System.ComponentModel.DataAnnotations;

namespace ProductManagement.BlazorApp.Components.Account.Models;

internal sealed class ChangeEmailInputModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "New email")]
    public string? UpdatedEmail { get; set; }
}
