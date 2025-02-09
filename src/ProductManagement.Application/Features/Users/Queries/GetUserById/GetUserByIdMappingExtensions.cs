using ProductManagement.Application.Models;

namespace ProductManagement.Application.Features.Users.Queries.GetUserById;

/// <summary>
/// Provides extension methods for mapping <see cref="GetUserByIdQuery"/> responses.
/// </summary>
internal static class GetUserByIdMappingExtensions
{
    public static GetUserByIdQueryResponse ToResponse(this ApplicationUserDto user)
    {
        return new GetUserByIdQueryResponse(user.Id,
                                            user.Email,
                                            user.EmailConfirmed,
                                            user.Role);
    }
}
