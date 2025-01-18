using ProductManagement.Domain.Entities;

namespace ProductManagement.Application.Products.Queries.GetProductsPaginated;

internal static class GetProductsPaginatedMappingExtensions
{
    public static GetProductsPaginatedQueryResponse ToQueryResponse(this Product product)
    {
        return new GetProductsPaginatedQueryResponse(
            product.Id,
            product.Name.Value,
            product.Description,
            product.IsActive,
            product.AddedOnUtc,
            product.Price.Value
        );
    }
}
