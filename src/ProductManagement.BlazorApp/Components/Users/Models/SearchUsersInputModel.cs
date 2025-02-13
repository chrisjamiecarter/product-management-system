using System.ComponentModel.DataAnnotations;

namespace ProductManagement.BlazorApp.Components.Users.Models;

/// <summary>
/// Represents the input model for searching users.
/// </summary>
public class SearchUsersInputModel
{
    [DataType(DataType.Text)]
    public string? SearchEmail { get; set; }

    public bool? SearchEmailConfirmed { get; set; }

    [DataType(DataType.Text)]
    public string? SearchRole { get; set; }

    public string? SortColumn { get; set; }

    public string? SortOrder { get; set; }
}
