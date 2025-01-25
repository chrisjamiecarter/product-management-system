namespace ProductManagement.Application.Features.Products.Queries.GetProducts;

public sealed record GetProductsQueryResponseProduct(
    Guid Id,
    string Name,
    string Description,
    bool IsActive,
    DateOnly AddedOnUtc,
    decimal Price);

