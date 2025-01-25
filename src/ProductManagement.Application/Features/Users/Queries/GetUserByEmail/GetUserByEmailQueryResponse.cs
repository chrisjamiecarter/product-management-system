namespace ProductManagement.Application.Features.Users.Queries.GetUserByEmail;

public sealed record GetUserByEmailQueryResponse(string Id,
                                                 string? Username,
                                                 string? Role,
                                                 bool EmailConfirmed);
