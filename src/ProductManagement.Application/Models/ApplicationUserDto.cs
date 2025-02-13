namespace ProductManagement.Application.Models;

/// <summary>
/// Represents a user in the application.
/// </summary>
public class ApplicationUserDto
{
    public ApplicationUserDto(string id, string? email, bool emailConfirmed, string? role)
    {
        Id = id;
        Email = email;
        EmailConfirmed = emailConfirmed;
        Role = role;
    }

    public string Id { get; set; }
    
    public string? Email { get; set; }
    
    public bool EmailConfirmed { get; set; }
    
    public string? Role { get; set; }
}
