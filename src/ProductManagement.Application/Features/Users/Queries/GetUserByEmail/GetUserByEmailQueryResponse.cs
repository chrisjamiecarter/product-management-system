namespace ProductManagement.Application.Features.Users.Queries.GetUserByEmail;

/// <summary>
/// Represents a response from a <see cref="GetUserByEmailQuery"/>.
/// </summary>
public sealed record GetUserByEmailQueryResponse(string Id,
                                                 string? Email,
                                                 bool EmailConfirmed,
                                                 string? Role);
