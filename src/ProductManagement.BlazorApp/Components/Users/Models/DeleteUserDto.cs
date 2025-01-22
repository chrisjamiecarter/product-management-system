using System.ComponentModel.DataAnnotations;
using ProductManagement.Application.Users.Queries.GetUserById;

namespace ProductManagement.BlazorApp.Components.Users.Models;

public class DeleteUserDto
{
    public DeleteUserDto(GetUserByIdQueryResponse user)
    {
        Id = user.Id;
        Username = user.Username;
        EmailConfirmed = user.EmailConfirmed;
    }

    [Editable(false)]
    public string Id { get; set; }

    [Editable(false)]
    [DataType(DataType.EmailAddress)]
    public string? Username { get; set; }

    [Editable(false)]
    public bool EmailConfirmed { get; set; }
}
