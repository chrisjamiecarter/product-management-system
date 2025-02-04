namespace ProductManagement.Application.Features.Users.Queries.GetUserByEmail;

public sealed record GetUserByEmailQueryResponse(string Id,
                                                 string? Email,
                                                 string? Role,
                                                 bool EmailConfirmed);
