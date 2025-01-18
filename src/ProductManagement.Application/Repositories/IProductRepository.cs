using ProductManagement.Application.Models;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Application.Repositories;

public interface IProductRepository
{
    Task<bool> CreateAsync(Product product, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> ReturnAllAsync(CancellationToken cancellationToken = default);
    Task<Product?> ReturnByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PaginatedList<Product>> ReturnByPageAsync(int PageNumber, int PageSize, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Product product, CancellationToken cancellationToken = default);
}
