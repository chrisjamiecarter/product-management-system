using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Application.Models;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Shared;
using ProductManagement.Infrastructure.Contexts;

namespace ProductManagement.Infrastructure.Repositories;

/// <summary>
/// Provides the repository for product operations.
/// </summary>
/// <param name="context">The database context object.</param>
/// <param name="logger">The logger object.</param>
internal class ProductRepository(ProductManagementDbContext context, ILogger<ProductRepository> logger) : IProductRepository
{
    public async Task<Result> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        await context.Products.AddAsync(product, cancellationToken);
        var created = await SaveAsync(cancellationToken);
        return created > 0
            ? Result.Success()
            : Result.Failure(ApplicationErrors.Product.NotCreated);
    }

    public async Task<Result> DeleteAsync(Product product, CancellationToken cancellationToken = default)
    {
        context.Products.Remove(product);
        var deleted = await SaveAsync(cancellationToken);
        return deleted > 0
            ? Result.Success()
            : Result.Failure(ApplicationErrors.Product.NotDeleted(product.Id));
    }

    public async Task<Result<Product>> ReturnByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await context.Products.FindAsync(id, cancellationToken);
        return product != null
            ? Result.Success(product)
            : Result.Failure<Product>(ApplicationErrors.Product.NotFound(id));
    }

    public async Task<Result<PaginatedList<Product>>> ReturnByPageAsync(string? searchName,
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
        if (pageNumber <= 0)
        {
            return Result.Failure<PaginatedList<Product>>(ApplicationErrors.PaginatedList.InvalidPageNumber(pageNumber));
        }

        if (pageSize <= 0)
        {
            return Result.Failure<PaginatedList<Product>>(ApplicationErrors.PaginatedList.InvalidPageSize(pageSize));
        }

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

        return Result.Success(PaginatedList<Product>.Create(items, count, pageNumber, pageSize));
    }

    public async Task<Result> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        context.Products.Update(product);
        var updated = await SaveAsync(cancellationToken);
        return updated > 0
            ? Result.Success()
            : Result.Failure(ApplicationErrors.Product.NotUpdated(product.Id));
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
            logger.LogWarning("{Exception}", exception.Message);
            return 0;
        }
    }
}
