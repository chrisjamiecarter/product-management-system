using System.ComponentModel.DataAnnotations;

namespace ProductManagement.BlazorApp.Components.Users.Models;

public class CreateUserDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string Username { get; set; } = string.Empty;

}
