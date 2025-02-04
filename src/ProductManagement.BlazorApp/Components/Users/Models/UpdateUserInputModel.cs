using System.ComponentModel.DataAnnotations;
using ProductManagement.Application.Features.Users.Queries.GetUserById;

namespace ProductManagement.BlazorApp.Components.Users.Models;

public class UpdateUserInputModel
{
    public UpdateUserInputModel(GetUserByIdQueryResponse user)
    {
        Id = user.Id;
        Email = user.Email;
        EmailConfirmed = user.EmailConfirmed;
        Role = user.Role;
    }

    [Required]
    [Editable(false)]
    public string Id { get; set; }

    [Required]
    [Editable(false)]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string? Email { get; set; }

    [Editable(false)]
    public bool EmailConfirmed { get; set; }

    [DataType(DataType.Text)]
    public string? Role { get; set; } = string.Empty;
}
