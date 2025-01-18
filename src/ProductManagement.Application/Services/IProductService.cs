using ProductManagement.Application.Models;
using ProductManagement.Application.Products.Commands.CreateProduct;
using ProductManagement.Application.Products.Commands.DeleteProduct;
using ProductManagement.Application.Products.Commands.UpdateProduct;
using ProductManagement.Application.Products.Queries.GetProductById;
using ProductManagement.Application.Products.Queries.GetProducts;
using ProductManagement.Application.Products.Queries.GetProductsPaginated;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Services;

public interface IProductService
{
    Task<Result> CreateAsync(CreateProductCommand command, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(DeleteProductCommand command, CancellationToken cancellationToken = default);
    Task<Result<GetProductsQueryResponse>> ReturnAllAsync(GetProductsQuery query, CancellationToken cancellationToken = default);
    Task<Result<GetProductByIdQueryResponse>> ReturnByIdAsync(GetProductByIdQuery query, CancellationToken cancellationToken = default);
    Task<Result<PaginatedList<GetProductsPaginatedQueryResponse>>> ReturnByPageAsync(GetProductsPaginatedQuery query, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(UpdateProductCommand command, CancellationToken cancellationToken = default);
}
