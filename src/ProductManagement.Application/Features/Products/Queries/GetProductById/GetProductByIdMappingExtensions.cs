using ProductManagement.Domain.Entities;

namespace ProductManagement.Application.Features.Products.Queries.GetProductById;

/// <summary>
/// Provides extension methods for mapping <see cref="GetProductByIdQuery"/> responses.
/// </summary>
internal static class GetProductByIdMappingExtensions
{
    public static GetProductByIdQueryResponse ToResponse(this Product product)
    {
        return new GetProductByIdQueryResponse(product.Id,
                                               product.Name.Value,
                                               product.Description,
                                               product.IsActive,
                                               product.AddedOnUtc,
                                               product.Price.Value);
    }
}
