using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Products.Commands.DeleteProduct;

/// <summary>
/// Handles the <see cref="DeleteProductCommand"/> by deleting an existing product.
/// </summary>
internal class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly ILogger<DeleteProductCommandHandler> _logger;
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(ILogger<DeleteProductCommandHandler> logger, IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var productResult = await _productRepository.ReturnByIdAsync(request.ProductId, cancellationToken);
        if (productResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", productResult.Error);
            return Result.Failure(productResult.Error);
        }

        var deleteResult = await _productRepository.DeleteAsync(productResult.Value, cancellationToken);
        if (deleteResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", deleteResult.Error);
            return Result.Failure(deleteResult.Error);
        }

        _logger.LogInformation("Deleted Product {id} successfully", request.ProductId);
        return Result.Success();
    }
}
