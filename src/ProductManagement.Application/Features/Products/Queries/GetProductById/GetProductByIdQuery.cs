using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Products.Queries.GetProductById;

/// <summary>
/// Represents a query to get an existing product by ID.
/// </summary>
public sealed record GetProductByIdQuery(Guid ProductId) : IQuery<GetProductByIdQueryResponse>;
