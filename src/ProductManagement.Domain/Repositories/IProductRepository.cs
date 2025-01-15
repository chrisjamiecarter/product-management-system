using ProductManagement.Domain.Entities;

namespace ProductManagement.Domain.Repositories;

public interface IProductRepository
{
    Task<bool> CreateAsync(Product product, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> ReturnAllAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Product?> ReturnByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Product product, CancellationToken cancellationToken = default);
}
