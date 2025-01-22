using System.ComponentModel.DataAnnotations;
using ProductManagement.Application.Users.Queries.GetUserById;

namespace ProductManagement.BlazorApp.Components.Users.Models;

public class UpdateUserDto
{
    public UpdateUserDto(GetUserByIdQueryResponse user)
    {
        Id = user.Id;
        Username = user.Username;
        EmailConfirmed = user.EmailConfirmed;
    }

    [Required]
    [Editable(false)]
    public string Id { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string? Username { get; set; }

    [Editable(false)]
    public bool EmailConfirmed { get; set; }
}
