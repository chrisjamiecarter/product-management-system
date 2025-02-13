using ProductManagement.Application.Models;
using ProductManagement.Infrastructure.Models;

namespace ProductManagement.Infrastructure.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="ApplicationUser"/> class.
/// </summary>
internal static class ApplicationUserExtensions
{
    public static ApplicationUserDto ToDto(this ApplicationUser user, string? role)
    {
        return new ApplicationUserDto(user.Id, user.Email, user.EmailConfirmed, role);
    }
}