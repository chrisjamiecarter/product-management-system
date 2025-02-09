using ProductManagement.Application.Models;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Interfaces.Infrastructure;

public interface IProductRepository
{
    Task<Result> CreateAsync(Product product, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Product product, CancellationToken cancellationToken = default);
    Task<Result<Product>> ReturnByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<PaginatedList<Product>>> ReturnByPageAsync(string? searchName,
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
    Task<Result> UpdateAsync(Product product, CancellationToken cancellationToken = default);
}
