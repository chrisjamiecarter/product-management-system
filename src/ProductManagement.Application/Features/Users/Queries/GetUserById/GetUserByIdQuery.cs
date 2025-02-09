using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Users.Queries.GetUserById;

/// <summary>
/// Represents a query to get an existing user by ID.
/// </summary>
public sealed record GetUserByIdQuery(string UserId) : IQuery<GetUserByIdQueryResponse>;
