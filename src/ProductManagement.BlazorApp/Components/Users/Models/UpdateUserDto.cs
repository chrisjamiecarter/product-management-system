using System.ComponentModel.DataAnnotations;
using ProductManagement.Application.Features.Users.Queries.GetUserById;

namespace ProductManagement.BlazorApp.Components.Users.Models;

public class UpdateUserDto
{
    public UpdateUserDto(GetUserByIdQueryResponse user)
    {
        Id = user.Id;
        Username = user.Username;
        Role = user.Role;
        EmailConfirmed = user.EmailConfirmed;
    }

    [Required]
    [Editable(false)]
    public string Id { get; set; }

    [Required]
    [Editable(false)]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string? Username { get; set; }

    [Required]
    [DataType(DataType.Text)]
    public string? Role { get; set; } = string.Empty;

    [Editable(false)]
    public bool EmailConfirmed { get; set; }
}
