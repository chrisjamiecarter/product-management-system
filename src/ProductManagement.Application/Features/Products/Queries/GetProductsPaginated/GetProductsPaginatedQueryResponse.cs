namespace ProductManagement.Application.Features.Products.Queries.GetProductsPaginated;

/// <summary>
/// Represents a response from a <see cref="GetProductsPaginatedQuery"/>.
/// </summary>
public sealed record GetProductsPaginatedQueryResponse(Guid Id,
                                                       string Name,
                                                       string Description,
                                                       bool IsActive,
                                                       DateOnly AddedOnUtc,
                                                       decimal Price);
