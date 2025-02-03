using Microsoft.AspNetCore.Identity;
using ProductManagement.Application.Models;

namespace ProductManagement.Infrastructure.Models;

internal class ApplicationUser : IdentityUser
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