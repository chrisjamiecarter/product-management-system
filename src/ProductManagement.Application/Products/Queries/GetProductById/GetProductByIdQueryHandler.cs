﻿using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Domain.Repositories;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Products.Queries.GetProductById;

internal sealed class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductResponse>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var id = request.ProductId;

        var product = await _productRepository.ReturnByIdAsync(id, cancellationToken);

        if (product is null)
        {
            return Result.Failure<ProductResponse>(ApplicationErrors.Product.NotFound);
        }

        var response = new ProductResponse(product.Id, product.Name.Value, product.Description, product.IsActive, product.AddedOnUtc, product.Price.Value);
        return Result.Success(response);
    }
}
