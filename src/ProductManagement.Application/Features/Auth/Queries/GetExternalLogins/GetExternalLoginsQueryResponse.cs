using ProductManagement.Application.Models;

namespace ProductManagement.Application.Features.Auth.Queries.GetExternalLogins;

/// <summary>
/// Represents a response from a <see cref="GetExternalLoginsQuery"/>.
/// </summary>
public sealed record GetExternalLoginsQueryResponse(IList<ExternalLoginDto> ExternalLogins);
