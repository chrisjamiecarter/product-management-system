using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Application.Features.Products.Commands.UpdateProduct;

/// <summary>
/// Handles the <see cref="UpdateProductCommand"/> by updating an existing product.
/// </summary>
internal sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly ILogger<UpdateProductCommandHandler> _logger;
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(ILogger<UpdateProductCommandHandler> logger, IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var nameResult = ProductName.Create(request.Name);
        if (nameResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", nameResult.Error);
            return Result.Failure(nameResult.Error);
        }

        var priceResult = ProductPrice.Create(request.Price);
        if (priceResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", priceResult.Error);
            return Result.Failure(priceResult.Error);
        }

        var productResult = await _productRepository.ReturnByIdAsync(request.ProductId, cancellationToken);
        if (productResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", productResult.Error);
            return Result.Failure(productResult.Error);
        }

        var product = productResult.Value;
        product.Name = nameResult.Value;
        product.Description = request.Description;
        product.IsActive = request.IsActive;
        product.Price = priceResult.Value;

        var updateResult = await _productRepository.UpdateAsync(product, cancellationToken);
        if (updateResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", updateResult.Error);
            return Result.Failure(updateResult.Error);
        }

        _logger.LogInformation("Updated Product {id} successfully", request.ProductId);
        return Result.Success();
    }
}
