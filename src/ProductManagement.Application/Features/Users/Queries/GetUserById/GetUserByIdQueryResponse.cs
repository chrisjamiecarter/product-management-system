namespace ProductManagement.Application.Features.Users.Queries.GetUserById;

public sealed record GetUserByIdQueryResponse(string Id,
                                              string? Email,
                                              string? Role,
                                              bool EmailConfirmed);
