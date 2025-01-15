using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.Database.Configurations;

namespace ProductManagement.Infrastructure.Database.Contexts;

internal class ProductManagementDbContext(DbContextOptions<ProductManagementDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
    }
}
