using ProductManagement.Application.Models;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Application.Interfaces.Infrastructure;

public interface IProductRepository
{
    Task<bool> CreateAsync(Product product, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> ReturnAllAsync(CancellationToken cancellationToken = default);
    Task<Product?> ReturnByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PaginatedList<Product>> ReturnByPageAsync(string? searchName,
                                                   bool? searchIsActive,
                                                   DateOnly? searchFromAddedOnDateUtc,
                                                   DateOnly? searchToAddedOnDateUtc,
                                                   decimal? searchFromPrice,
                                                   decimal? searchToPrice,
                                                   string? sortColumn,
                                                   string? sortOrder,
                                                   int pageNumber,
                                                   int pageSize,
                                                   CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Product product, CancellationToken cancellationToken = default);
}
