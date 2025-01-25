using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid ProductId) : IQuery<GetProductByIdQueryResponse>;
