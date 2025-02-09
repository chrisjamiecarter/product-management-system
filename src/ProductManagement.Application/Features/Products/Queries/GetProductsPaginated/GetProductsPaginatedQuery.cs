using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Models;

namespace ProductManagement.Application.Features.Products.Queries.GetProductsPaginated;

/// <summary>
/// Represents a query to get a page of products.
/// </summary>
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
