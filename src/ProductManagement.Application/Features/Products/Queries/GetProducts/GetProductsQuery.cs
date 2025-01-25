using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Products.Queries.GetProducts;

public sealed record GetProductsQuery() : IQuery<GetProductsQueryResponse>;
