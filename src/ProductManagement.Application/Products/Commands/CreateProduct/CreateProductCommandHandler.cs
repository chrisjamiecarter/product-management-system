using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Repositories;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Shared;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Application.Products.Commands.CreateProduct;

internal sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var name = ProductName.Create(request.Name);
        var description = request.Description;
        var price = ProductPrice.Create(request.Price);

        if (name.IsFailure)
        {
            return Result.Failure(name.Error);
        }

        if (price.IsFailure)
        {
            return Result.Failure(price.Error);
        }

        var product = new Product(
            Guid.CreateVersion7(),
            name.Value,
            description,
            price.Value);

        var isCreated = await _productRepository.CreateAsync(product, cancellationToken);

        return isCreated ? Result.Success() : Result.Failure(ApplicationErrors.Product.NotCreated);
    }
}
