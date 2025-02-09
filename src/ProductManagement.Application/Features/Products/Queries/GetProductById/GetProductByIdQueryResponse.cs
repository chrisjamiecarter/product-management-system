namespace ProductManagement.Application.Features.Products.Queries.GetProductById;

/// <summary>
/// Represents a response from a <see cref="GetProductByIdQuery"/>.
/// </summary>
public sealed record GetProductByIdQueryResponse(Guid Id,
                                                 string Name,
                                                 string Description,
                                                 bool IsActive,
                                                 DateOnly AddedOnUtc,
                                                 decimal Price);
