using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.Database.Configurations;
using ProductManagement.Infrastructure.Database.Constants;
using ProductManagement.Infrastructure.Database.Models;

namespace ProductManagement.Infrastructure.Database.Contexts;

internal class ProductManagementDbContext(DbContextOptions<ProductManagementDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Product> Products { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(a =>
        {
            a.ToTable("Users", SchemaConstants.IdentitySchema);
            a.HasMany(e => e.UserRoles).WithOne().HasForeignKey(fk => fk.UserId).IsRequired();
        });
            
        builder.Entity<IdentityRole>().ToTable("Roles", SchemaConstants.IdentitySchema);
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", SchemaConstants.IdentitySchema);
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", SchemaConstants.IdentitySchema);
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", SchemaConstants.IdentitySchema);
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", SchemaConstants.IdentitySchema);
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", SchemaConstants.IdentitySchema);

        builder.ApplyConfiguration(new ProductConfiguration());
    }
}
