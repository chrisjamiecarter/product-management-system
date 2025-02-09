namespace ProductManagement.Application.Features.Users.Queries.GetUsersPaginated;

/// <summary>
/// Represents a response from a <see cref="GetUsersPaginatedQuery"/>.
/// </summary>
public sealed record GetUsersPaginatedQueryResponse(string Id,
                                                    string? Email,
                                                    bool EmailConfirmed,
                                                    string? Role);
