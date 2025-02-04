namespace ProductManagement.Application.Features.Users.Queries.GetUsersPaginated;

public sealed record GetUsersPaginatedQueryResponse(string Id,
                                                    string? Email,
                                                    bool EmailConfirmed,
                                                    string? Role);
