namespace ProductManagement.Application.Users.Queries.GetUsersPaginated;

public sealed record GetUsersPaginatedQueryResponse(string Id,
                                                    string? UserName,
                                                    bool EmailConfirmed);
