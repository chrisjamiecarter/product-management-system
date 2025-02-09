using ProductManagement.Application.Models;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Application.Features.Products.Queries.GetProductsPaginated;

/// <summary>
/// Provides extension methods for mapping <see cref="GetProductsPaginatedQuery"/> responses.
/// </summary>
internal static class GetProductsPaginatedMappingExtensions
{
    public static PaginatedList<GetProductsPaginatedQueryResponse> ToResponse(this PaginatedList<Product> products)
    {
        return PaginatedList<GetProductsPaginatedQueryResponse>.Create(products.Items.Select(p => p.ToResponse()),
                                                                       products.TotalCount,
                                                                       products.PageNumber,
                                                                       products.PageSize);
    }

    public static GetProductsPaginatedQueryResponse ToResponse(this Product product)
    {
        return new GetProductsPaginatedQueryResponse(product.Id,
                                                     product.Name.Value,
                                                     product.Description,
                                                     product.IsActive,
                                                     product.AddedOnUtc,
                                                     product.Price.Value);
    }
}
