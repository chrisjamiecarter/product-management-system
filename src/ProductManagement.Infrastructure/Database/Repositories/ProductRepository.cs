using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.Models;
using ProductManagement.Application.Repositories;
using ProductManagement.Domain.Entities;
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

    public async Task<IReadOnlyList<Product>> ReturnAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Products.ToListAsync(cancellationToken);
    }

    public async Task<Product?> ReturnByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Products.FindAsync(id, cancellationToken);
    }

    public async Task<PaginatedList<Product>> ReturnByPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = context.Products.AsQueryable();
        
        var count = await query.CountAsync(cancellationToken);
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return PaginatedList<Product>.Create(items, count, pageNumber, pageSize);
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
