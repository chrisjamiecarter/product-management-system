namespace ProductManagement.Application.Models;

public class ApplicationUserDto
{
    public ApplicationUserDto(string id, string? username, string? role, bool emailConfirmed)
    {
        Id = id;
        Username = username;
        Role = role;
        EmailConfirmed = emailConfirmed;
    }

    public string Id { get; set; }
    
    public string? Username { get; set; }
    
    public string? Role { get; set; }
    
    public bool EmailConfirmed { get; set; }
}
