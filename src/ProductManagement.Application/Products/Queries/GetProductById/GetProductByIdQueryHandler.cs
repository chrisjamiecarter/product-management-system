using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Repositories;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Products.Queries.GetProductById;

internal sealed class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, GetProductByIdQueryResponse>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<GetProductByIdQueryResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.ReturnByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
        {
            return Result.Failure<GetProductByIdQueryResponse>(ApplicationErrors.Product.NotFound);
        }

        var response = new GetProductByIdQueryResponse(product.Id, product.Name.Value, product.Description, product.IsActive, product.AddedOnUtc, product.Price.Value);
        return Result.Success(response);
    }
}
