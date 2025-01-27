using System.ComponentModel.DataAnnotations;

namespace ProductManagement.BlazorApp.Components.Users.Models;

public class CreateUserInputModel
{
    [Required]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [DataType(DataType.Text)]
    public string Role { get; set; } = string.Empty;

}
