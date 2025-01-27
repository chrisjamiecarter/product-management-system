using Microsoft.AspNetCore.Identity;

namespace ProductManagement.Infrastructure.Database.Models;

public class ApplicationUser : IdentityUser
{
    public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; } = default!;
}
