using ProductManagement.BlazorApp.Enums;

namespace ProductManagement.BlazorApp.Components.Account.Shared;

public class AuthError
{
    public string? Message { get; set; }

    public MessageLevel? Level { get; set; }
}
