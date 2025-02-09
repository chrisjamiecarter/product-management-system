namespace ProductManagement.Application.Features.Users.Queries.GetUserById;

/// <summary>
/// Represents a response from a <see cref="GetUserByIdQuery"/>.
/// </summary>
public sealed record GetUserByIdQueryResponse(string Id,
                                              string? Email,
                                              bool EmailConfirmed,
                                              string? Role);
