using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid ProductId) : IQuery<ProductResponse>;
