namespace ProductManagement.Application.Products.Queries.GetProductById;

public sealed record ProductResponse(
    Guid Id,
    string Name,
    string Description,
    bool IsActive,
    DateOnly AddedOnUtc,
    decimal Price);
