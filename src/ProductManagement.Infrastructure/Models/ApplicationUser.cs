using Microsoft.AspNetCore.Identity;
using ProductManagement.Application.Models;

namespace ProductManagement.Infrastructure.Models;

// TODO: This needs to be made internal and all Infrastructure references in the BlazorApp project need to be removed.
public class ApplicationUser : IdentityUser
{
    public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; } = [];
}

internal static class ApplicationUserExtensions
{
    public static ApplicationUserDto ToDto(this ApplicationUser user, string? role)
    {
        return new ApplicationUserDto(user.Id, user.UserName, role, user.EmailConfirmed);
    }
}