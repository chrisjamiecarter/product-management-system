using ProductManagement.Application.Features.Products.Commands.CreateProduct;
using ProductManagement.Application.Features.Products.Commands.DeleteProduct;
using ProductManagement.Application.Features.Products.Commands.UpdateProduct;
using ProductManagement.Application.Features.Products.Queries.GetProductById;
using ProductManagement.Application.Features.Products.Queries.GetProducts;
using ProductManagement.Application.Features.Products.Queries.GetProductsPaginated;
using ProductManagement.Application.Models;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Interfaces.Application;

public interface IProductService
{
    Task<Result> CreateAsync(CreateProductCommand command, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(DeleteProductCommand command, CancellationToken cancellationToken = default);
    Task<Result<GetProductsQueryResponse>> ReturnAllAsync(GetProductsQuery query, CancellationToken cancellationToken = default);
    Task<Result<GetProductByIdQueryResponse>> ReturnByIdAsync(GetProductByIdQuery query, CancellationToken cancellationToken = default);
    Task<Result<PaginatedList<GetProductsPaginatedQueryResponse>>> ReturnByPageAsync(GetProductsPaginatedQuery query, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(UpdateProductCommand command, CancellationToken cancellationToken = default);
}
