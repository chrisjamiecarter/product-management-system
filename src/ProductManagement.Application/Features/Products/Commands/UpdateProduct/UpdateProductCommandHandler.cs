using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Application.Features.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var name = ProductName.Create(request.Name);
        if (name.IsFailure)
        {
            return Result.Failure(name.Error);
        }

        var price = ProductPrice.Create(request.Price);
        if (price.IsFailure)
        {
            return Result.Failure(price.Error);
        }

        var product = await _productRepository.ReturnByIdAsync(request.Id, cancellationToken);
        if (product is null)
        {
            return Result.Failure(ApplicationErrors.Product.NotFound);
        }

        product.Name = name.Value;
        product.Description = request.Description;
        product.IsActive = request.IsActive;
        product.Price = price.Value;

        var isUpdated = await _productRepository.UpdateAsync(product, cancellationToken);

        return isUpdated ? Result.Success() : Result.Failure(ApplicationErrors.Product.NotUpdated);
    }
}
