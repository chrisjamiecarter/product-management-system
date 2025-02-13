using System.ComponentModel.DataAnnotations;
using ProductManagement.Application.Features.Users.Queries.GetUserById;

namespace ProductManagement.BlazorApp.Components.Users.Models;

/// <summary>
/// Represents the input model for deleting an existing user.
/// </summary>
public class DeleteUserInputModel
{
    public DeleteUserInputModel(GetUserByIdQueryResponse user)
    {
        Id = user.Id;
        Email = user.Email;
        EmailConfirmed = user.EmailConfirmed;
        Role = user.Role;
    }

    [Editable(false)]
    public string Id { get; set; }

    [Editable(false)]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [Editable(false)]
    public bool EmailConfirmed { get; set; }

    [Editable(false)]
    [DataType(DataType.Text)]
    public string? Role { get; set; } = string.Empty;
}
