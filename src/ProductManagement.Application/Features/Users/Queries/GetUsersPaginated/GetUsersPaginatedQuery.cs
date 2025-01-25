using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Models;

namespace ProductManagement.Application.Features.Users.Queries.GetUsersPaginated;

public sealed record GetUsersPaginatedQuery(string? SearchUsername,
                                            string? SearchRole,
                                            bool? SearchEmailConfirmed,
                                            string? SortColumn,
                                            string? SortOrder,
                                            int PageNumber = 1,
                                            int PageSize = 10) : IQuery<PaginatedList<GetUsersPaginatedQueryResponse>>;
