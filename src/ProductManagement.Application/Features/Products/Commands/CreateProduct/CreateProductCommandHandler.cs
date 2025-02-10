using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Shared;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Application.Features.Products.Commands.CreateProduct;

/// <summary>
/// Handles the <see cref="CreateProductCommand"/> by creating a new product.
/// </summary>
internal sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
{
    private readonly ILogger<CreateProductCommandHandler> _logger;
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(ILogger<CreateProductCommandHandler> logger, IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
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

        var product = new Product(Guid.CreateVersion7(),
                                  nameResult.Value,
                                  request.Description,
                                  priceResult.Value);

        var createResult = await _productRepository.CreateAsync(product, cancellationToken);
        if (createResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", createResult.Error);
            return Result.Failure(createResult.Error);
        }

        _logger.LogInformation("Created Product {id} successfully", product.Id);
        return Result.Success();
    }
}
