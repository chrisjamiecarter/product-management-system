using ProductManagement.Application.Models;

namespace ProductManagement.Application.Features.Auth.Queries.GetExternalLogins;

/// <summary>
/// Provides extension methods for mapping <see cref="GetExternalLoginsQuery"/> responses.
/// </summary>
internal static class GetExternalLoginsMappingExtensions
{
    public static GetExternalLoginsQueryResponse ToResponse(this List<ExternalLoginDto> externalLogins)
    {
        return new GetExternalLoginsQueryResponse(externalLogins);
    }
}
