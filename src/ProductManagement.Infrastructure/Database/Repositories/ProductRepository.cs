using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Repositories;
using ProductManagement.Infrastructure.Database.Contexts;

namespace ProductManagement.Infrastructure.Database.Repositories;

internal class ProductRepository(ProductManagementDbContext context, ILogger<ProductRepository> logger) : IProductRepository
{
    public async Task<bool> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        await context.Products.AddAsync(product, cancellationToken);
        var created = await SaveAsync(cancellationToken);
        return created > 0;
    }

    public async Task<bool> DeleteAsync(Product product, CancellationToken cancellationToken = default)
    {
        context.Products.Remove(product);
        var deleted = await SaveAsync(cancellationToken);
        return deleted > 0;
    }

    public async Task<IReadOnlyList<Product>> ReturnAllAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Products.ToListAsync(cancellationToken);
    }

    public async Task<Product?> ReturnByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Products.FindAsync(id, cancellationToken);
    }

    public async Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        context.Products.Update(product);
        var updated = await SaveAsync(cancellationToken);
        return updated > 0;
    }

    private async Task<int> SaveAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogWarning("Exception when saving changes: {exceptionMessage}", exception.Message);
            return 0;
        }
    }
}
