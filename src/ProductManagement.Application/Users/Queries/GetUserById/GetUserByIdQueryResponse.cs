namespace ProductManagement.Application.Users.Queries.GetUserById;

public sealed record GetUserByIdQueryResponse(string Id,
                                              string? Username,
                                              bool EmailConfirmed);
