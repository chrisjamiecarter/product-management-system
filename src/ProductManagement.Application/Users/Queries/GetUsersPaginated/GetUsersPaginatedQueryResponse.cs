namespace ProductManagement.Application.Users.Queries.GetUsersPaginated;

public sealed record GetUsersPaginatedQueryResponse(string Id,
                                                    string? Username,
                                                    string? Role,
                                                    bool EmailConfirmed);
