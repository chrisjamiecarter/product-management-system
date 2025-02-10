using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Products.Queries.GetProductById;

/// <summary>
/// Handles the <see cref="GetProductByIdQuery"/> by retrieving a single product by its ID, and returns a <see cref="GetProductByIdQueryResponse"/>.
/// </summary>
internal sealed class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, GetProductByIdQueryResponse>
{
    private readonly ILogger<GetProductByIdQueryHandler> _logger;
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(ILogger<GetProductByIdQueryHandler> logger, IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }

    public async Task<Result<GetProductByIdQueryResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _productRepository.ReturnByIdAsync(request.ProductId, cancellationToken);
        if (result.IsFailure)
        {
            _logger.LogWarning("{@Error}", result.Error);
            return Result.Failure<GetProductByIdQueryResponse>(result.Error);
        }

        var response = result.Value.ToResponse();
        _logger.LogInformation("Returned Product {id} successfully", request.ProductId);
        return Result.Success(response);
    }
}
