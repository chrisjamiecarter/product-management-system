using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Models;

namespace ProductManagement.Application.Products.Queries.GetProductsPaginated;

public sealed record GetProductsPaginatedQuery(
    int PageNumber = 1,
    int PageSize = 10) : IQuery<PaginatedList<GetProductsPaginatedQueryResponse>>;
