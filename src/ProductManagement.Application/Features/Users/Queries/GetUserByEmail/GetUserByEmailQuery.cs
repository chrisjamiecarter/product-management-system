using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Users.Queries.GetUserByEmail;

/// <summary>
/// Represents a query to get an existing user by email.
/// </summary>
public sealed record GetUserByEmailQuery(string Email) : IQuery<GetUserByEmailQueryResponse>;
