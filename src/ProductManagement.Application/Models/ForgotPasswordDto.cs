using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Application.Models;

public sealed class ForgotPasswordDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
