namespace ProductManagement.Application.Models;

public class User
{
    public string Id { get; set; }
    public string? UserName { get; set; }
    public bool EmailConfirmed { get; set; }
    //public IList<string> Roles { get; set; }
}
