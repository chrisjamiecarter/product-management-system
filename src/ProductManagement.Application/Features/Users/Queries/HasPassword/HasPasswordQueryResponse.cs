namespace ProductManagement.Application.Features.Users.Queries.HasPassword;

/// <summary>
/// Represents a response from a <see cref="HasPasswordQuery"/>.
/// </summary>
public sealed record HasPasswordQueryResponse(bool HasPassword);
