using ProductManagement.Application.Models;

namespace ProductManagement.Application.Features.Users.Queries.GetUsersPaginated;

internal static class GetUsersPaginatedMappingExtensions
{
    public static GetUsersPaginatedQueryResponse ToQueryResponse(this ApplicationUserDto user)
    {
        return new GetUsersPaginatedQueryResponse(user.Id,
                                                  user.Username,
                                                  user.Role,
                                                  user.EmailConfirmed);
    }
}
