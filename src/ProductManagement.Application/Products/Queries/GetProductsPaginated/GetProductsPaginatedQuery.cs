using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Models;

namespace ProductManagement.Application.Products.Queries.GetProductsPaginated;

public sealed record GetProductsPaginatedQuery(string? SearchName,
                                               bool? SearchIsActive,
                                               DateOnly? SearchFromAddedOnDateUtc,
                                               DateOnly? SearchToAddedOnDateUtc,
                                               decimal? SearchFromPrice,
                                               decimal? SearchToPrice,
                                               string? SortColumn,
                                               string? SortOrder,
                                               int PageNumber = 1,
                                               int PageSize = 10) : IQuery<PaginatedList<GetProductsPaginatedQueryResponse>>;
