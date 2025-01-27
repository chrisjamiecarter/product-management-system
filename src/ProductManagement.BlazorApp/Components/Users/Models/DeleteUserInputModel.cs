using System.ComponentModel.DataAnnotations;
using ProductManagement.Application.Features.Users.Queries.GetUserById;

namespace ProductManagement.BlazorApp.Components.Users.Models;

public class DeleteUserInputModel
{
    public DeleteUserInputModel(GetUserByIdQueryResponse user)
    {
        Id = user.Id;
        Username = user.Username;
        Role = user.Role;
        EmailConfirmed = user.EmailConfirmed;
    }

    [Editable(false)]
    public string Id { get; set; }

    [Editable(false)]
    [DataType(DataType.EmailAddress)]
    public string? Username { get; set; }

    [Editable(false)]
    [DataType(DataType.Text)]
    public string? Role { get; set; } = string.Empty;

    [Editable(false)]
    public bool EmailConfirmed { get; set; }
}
