namespace ProductManagement.Application.Features.Auth.Queries.GetCurrentUser;

public sealed record GetCurrentUserQueryResponse(string UserId,
                                                 string? Email,
                                                 string? Role,
                                                 bool EmailConfirmed);
