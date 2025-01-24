using System.ComponentModel.DataAnnotations;

namespace ProductManagement.BlazorApp.Components.Users.Models;

public class SearchUsersDto
{
    [DataType(DataType.Text)]
    public string? SearchUsername { get; set; }

    [DataType(DataType.Text)]
    public string? SearchRole { get; set; }

    public bool? SearchEmailConfirmed { get; set; }

    public string? SortColumn { get; set; }

    public string? SortOrder { get; set; }
}
