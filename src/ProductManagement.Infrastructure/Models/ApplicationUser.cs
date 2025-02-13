using Microsoft.AspNetCore.Identity;

namespace ProductManagement.Infrastructure.Models;

/// <summary>
/// Represents a user in the application, extending the <see cref="IdentityUser"/> class.
/// </summary>
internal class ApplicationUser : IdentityUser
{
    public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; } = [];
}
