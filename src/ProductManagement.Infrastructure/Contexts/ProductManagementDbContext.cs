using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.Configurations;
using ProductManagement.Infrastructure.Models;

namespace ProductManagement.Infrastructure.Contexts;

/// <summary>
/// The Entity Framework database context for the application.
/// Configures the models and provides access to the database tables.
/// </summary>
/// <param name="options">The <see cref="DbContextOptions{ProductManagementDbContext}"/> used to configure the database context.</param>
internal class ProductManagementDbContext(DbContextOptions<ProductManagementDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<AuditLog> Logs { get; set; } = default!;

    public DbSet<Product> Products { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ApplicationUserConfiguration());
        builder.ApplyConfiguration(new AuditLogConfiguration());
        builder.ApplyConfiguration(new IdentityRoleConfiguration());
        builder.ApplyConfiguration(new IdentityUserClaimConfiguration());
        builder.ApplyConfiguration(new IdentityUserRoleConfiguration());
        builder.ApplyConfiguration(new IdentityUserLoginConfiguration());
        builder.ApplyConfiguration(new IdentityRoleClaimConfiguration());
        builder.ApplyConfiguration(new IdentityUserTokenConfiguration());
        builder.ApplyConfiguration(new ProductConfiguration());
    }
}
