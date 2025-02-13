namespace ProductManagement.Infrastructure.Models;

/// <summary>
/// Represents a user to be seeded into the database.
/// </summary>
internal class SeedUser
{
    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}
