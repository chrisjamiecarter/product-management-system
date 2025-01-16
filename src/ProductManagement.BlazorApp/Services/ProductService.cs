using MediatR;
using ProductManagement.Application.Products.Commands.CreateProduct;
using ProductManagement.Application.Products.Commands.DeleteProduct;
using ProductManagement.Application.Products.Commands.UpdateProduct;
using ProductManagement.Application.Products.Queries.GetProductById;
using ProductManagement.Application.Products.Queries.GetProducts;
using ProductManagement.Application.Services;
using ProductManagement.Domain.Shared;

namespace ProductManagement.BlazorApp.Services;

public class ProductService : IProductService
{
    private readonly ISender _sender;

    public ProductService(ISender sender)
    {
        _sender = sender;
    }

    public async Task<Result> CreateAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
    {
        return await _sender.Send(command, cancellationToken);
    }

    public async Task<Result> DeleteAsync(DeleteProductCommand command, CancellationToken cancellationToken = default)
    {
        return await _sender.Send(command, cancellationToken);
    }

    public async Task<Result<GetProductsQueryResponse>> ReturnAllAsync(GetProductsQuery query, CancellationToken cancellationToken = default)
    {
        return await _sender.Send(query, cancellationToken);
    }

    public async Task<Result<GetProductByIdQueryResponse>> ReturnByIdAsync(GetProductByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _sender.Send(query, cancellationToken);
    }

    public async Task<Result> UpdateAsync(UpdateProductCommand command, CancellationToken cancellationToken = default)
    {
        return await _sender.Send(command, cancellationToken);
    }
}
