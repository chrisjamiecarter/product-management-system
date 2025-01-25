namespace ProductManagement.Application.Features.Products.Queries.GetProductById;

public sealed record GetProductByIdQueryResponse(Guid Id,
                                                 string Name,
                                                 string Description,
                                                 bool IsActive,
                                                 DateOnly AddedOnUtc,
                                                 decimal Price);
