using ProductManagement.Application.Models;

namespace ProductManagement.Application.Features.Users.Queries.GetUserByEmail;

internal static class GetUserByEmailMappingExtensions
{
    public static ApplicationUserDto ToDto(this GetUserByEmailQueryResponse response)
    {
        return new ApplicationUserDto(response.Id,
                                      response.Email,
                                      response.EmailConfirmed,
                                      response.Role);
    }
}
