using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Repositories;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Products.Queries.GetProducts;

internal sealed class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, GetProductsQueryResponse>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<GetProductsQueryResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.ReturnAllAsync(cancellationToken);
        var response = new GetProductsQueryResponse(products.Select(x => new GetProductsQueryResponseProduct(x.Id, x.Name.Value, x.Description, x.IsActive, x.AddedOnUtc, x.Price.Value)).ToList());
        return Result.Success(response);
    }
}
