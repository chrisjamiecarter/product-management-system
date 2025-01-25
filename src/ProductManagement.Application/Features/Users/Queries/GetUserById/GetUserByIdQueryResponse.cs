namespace ProductManagement.Application.Features.Users.Queries.GetUserById;

public sealed record GetUserByIdQueryResponse(string Id,
                                              string? Username,
                                              string? Role,
                                              bool EmailConfirmed);
