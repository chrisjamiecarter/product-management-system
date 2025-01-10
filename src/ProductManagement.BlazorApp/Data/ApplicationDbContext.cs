using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProductManagement.BlazorApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    private static readonly string IdentitySchema = "security";

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>().ToTable("AspNetUsers", IdentitySchema);
        builder.Entity<IdentityRole>().ToTable("AspNetRoles", IdentitySchema);
        builder.Entity<IdentityUserClaim<string> > ().ToTable("AspNetUserClaims", IdentitySchema);
        builder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles", IdentitySchema);
        builder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins", IdentitySchema);
        builder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims", IdentitySchema);
        builder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens", IdentitySchema);
    }
}
