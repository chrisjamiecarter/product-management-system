using System.Linq.Expressions;
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

    public async Task<PaginatedList<Product>> ReturnByPageAsync(string? searchName,
                                                                bool? searchIsActive,
                                                                DateOnly? searchFromAddedOnDateUtc,
                                                                DateOnly? searchToAddedOnDateUtc,
                                                                decimal? searchFromPrice,
                                                                decimal? searchToPrice,
                                                                string? sortColumn,
                                                                string? sortOrder,
                                                                int pageNumber,
                                                                int pageSize,
                                                                CancellationToken cancellationToken = default)
    {
        var query = context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchName))
        {
            query = query.Where(p => ((string)p.Name).Contains(searchName));
        }

        if (searchIsActive != null)
        {
            query = query.Where(p => p.IsActive == searchIsActive);
        }

        if (searchFromAddedOnDateUtc != null)
        {
            query = query.Where(p => p.AddedOnUtc >= searchFromAddedOnDateUtc);
        }

        if (searchToAddedOnDateUtc != null)
        {
            query = query.Where(p => p.AddedOnUtc <= searchToAddedOnDateUtc);
        }

        if (searchFromPrice != null)
        {
            query = query.Where(p => (decimal)p.Price >= searchFromPrice);
        }

        if (searchToPrice != null)
        {
            query = query.Where(p => (decimal)p.Price <= searchToPrice);
        }

        if (sortOrder?.ToLower() == "desc")
        {
            query = query.OrderByDescending(GetSortProperty(sortColumn)).ThenBy(p => p.Name).ThenBy(p => p.AddedOnUtc);
        }
        else
        {
            query = query.OrderBy(GetSortProperty(sortColumn)).ThenBy(p => p.Name).ThenBy(p => p.AddedOnUtc);
        }

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

    private static Expression<Func<Product, object>> GetSortProperty(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "name" => product => product.Name,
            "addedonutc" => product => product.AddedOnUtc,
            "price" => product => product.Price,
            _ => product => product.Id,
        };
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
