using ProductManagement.Application.Models;

namespace ProductManagement.Application.Users.Queries.GetUsersPaginated;

internal static class GetUsersPaginatedMappingExtensions
{
    public static GetUsersPaginatedQueryResponse ToQueryResponse(this User user)
    {
        return new GetUsersPaginatedQueryResponse(user.Id,
                                                  user.UserName,
                                                  user.EmailConfirmed);
    }
}
