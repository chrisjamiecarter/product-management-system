using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Models;

namespace ProductManagement.Application.Features.Users.Queries.GetUsersPaginated;

/// <summary>
/// Represents a query to get a page of users.
/// </summary>
public sealed record GetUsersPaginatedQuery(string? SearchEmail,
                                            bool? SearchEmailConfirmed,
                                            string? SearchRole,
                                            string? SortColumn,
                                            string? SortOrder,
                                            int PageNumber = 1,
                                            int PageSize = 10) : IQuery<PaginatedList<GetUsersPaginatedQueryResponse>>;
