using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Queries.GetExternalLogins;

/// <summary>
/// Represents a query to get the external logins for an existing user.
/// </summary>
public sealed record GetExternalLoginsQuery(string Id) : IQuery<GetExternalLoginsQueryResponse>;
