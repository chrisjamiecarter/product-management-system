using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Users.Queries.HasPassword;

/// <summary>
/// Represents a query to determin if a user has set a password.
/// </summary>
public sealed record HasPasswordQuery(string UserId) : IQuery<HasPasswordQueryResponse>;
