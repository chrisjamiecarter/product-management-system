using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Domain.Repositories;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Products.Commands.DeleteProduct;

internal class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var id = request.Id;

        var product = await _productRepository.ReturnByIdAsync(id, cancellationToken);

        if (product is null)
        {
            return Result.Failure(ApplicationErrors.Product.NotFound);
        }

        var isDeleted = await _productRepository.DeleteAsync(product, cancellationToken);

        return isDeleted ? Result.Success() : Result.Failure(ApplicationErrors.Product.NotDeleted);
    }
}
