using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Products.Queries.GetProducts;

public sealed record GetProductsQuery() : IQuery<GetProductsQueryResponse>;
