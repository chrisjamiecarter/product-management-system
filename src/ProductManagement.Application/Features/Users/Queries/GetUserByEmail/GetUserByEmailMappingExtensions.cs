using ProductManagement.Application.Models;

namespace ProductManagement.Application.Features.Users.Queries.GetUserByEmail;

/// <summary>
/// Provides extension methods for mapping <see cref="GetUserByEmailQuery"/> responses.
/// </summary>
internal static class GetUserByEmailMappingExtensions
{
    public static GetUserByEmailQueryResponse ToResponse(this ApplicationUserDto user)
    {
        return new GetUserByEmailQueryResponse(user.Id,
                                               user.Email,
                                               user.EmailConfirmed,
                                               user.Role);
    }
}
