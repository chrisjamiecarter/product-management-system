namespace ProductManagement.Application.Features.Products.Queries.GetProductsPaginated;

public sealed record GetProductsPaginatedQueryResponse(Guid Id,
                                                       string Name,
                                                       string Description,
                                                       bool IsActive,
                                                       DateOnly AddedOnUtc,
                                                       decimal Price);
