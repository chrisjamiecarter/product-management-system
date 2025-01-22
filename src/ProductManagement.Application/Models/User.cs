namespace ProductManagement.Application.Models;

public class User
{
    public User(string id, string? username, bool emailConfirmed)
    {
        Id = id;
        Username = username;
        EmailConfirmed = emailConfirmed;
    }

    public string Id { get; set; }
    public string? Username { get; set; }
    public bool EmailConfirmed { get; set; }
    //public IList<string> Roles { get; set; }
}
