using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Models;

namespace ProductManagement.Application.Users.Queries.GetUsersPaginated;

public sealed record GetUsersPaginatedQuery(string? SearchUsername,
                                            bool? SearchEmailConfirmed,
                                            string? SortOrder,
                                            int PageNumber = 1,
                                            int PageSize = 10) : IQuery<PaginatedList<GetUsersPaginatedQueryResponse>>;
