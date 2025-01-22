using System.ComponentModel.DataAnnotations;

namespace ProductManagement.BlazorApp.Components.Users.Models;

public class SearchUsersDto
{
    [DataType(DataType.Text)]
    public string? SearchUsername { get; set; }

    public bool? SearchEmailConfirmed { get; set; }

    public string? SortOrder { get; set; }
}
