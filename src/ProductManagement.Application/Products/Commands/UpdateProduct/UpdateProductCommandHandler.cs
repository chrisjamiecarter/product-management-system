using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Domain.Repositories;
using ProductManagement.Domain.Shared;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Application.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var id = request.Id;
        var name = ProductName.Create(request.Name);
        var description = request.Description;
        var isActive = request.IsActive;
        var price = ProductPrice.Create(request.Price);

        if (name.IsFailure)
        {
            return Result.Failure(name.Error);
        }

        if (price.IsFailure)
        {
            return Result.Failure(price.Error);
        }

        var product = await _productRepository.ReturnByIdAsync(id, cancellationToken);

        if (product is null)
        {
            return Result.Failure(ApplicationErrors.Product.NotFound);
        }

        product.Name = name.Value;
        product.Description = description;
        product.IsActive = isActive;
        product.Price = price.Value;

        var isUpdated = await _productRepository.UpdateAsync(product, cancellationToken);

        return isUpdated ? Result.Success() : Result.Failure(ApplicationErrors.Product.NotUpdated);
    }
}
