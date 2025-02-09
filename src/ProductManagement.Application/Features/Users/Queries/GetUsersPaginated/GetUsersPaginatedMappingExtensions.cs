using ProductManagement.Application.Models;

namespace ProductManagement.Application.Features.Users.Queries.GetUsersPaginated;

/// <summary>
/// Provides extension methods for mapping <see cref="GetUsersPaginatedQuery"/> responses.
/// </summary>
internal static class GetUsersPaginatedMappingExtensions
{
    public static PaginatedList<GetUsersPaginatedQueryResponse> ToResponse(this PaginatedList<ApplicationUserDto> users)
    {
        return PaginatedList<GetUsersPaginatedQueryResponse>.Create(users.Items.Select(u => u.ToResponse()),
                                                                    users.TotalCount,
                                                                    users.PageNumber,
                                                                    users.PageSize);
    }

    public static GetUsersPaginatedQueryResponse ToResponse(this ApplicationUserDto user)
    {
        return new GetUsersPaginatedQueryResponse(user.Id,
                                                  user.Email,
                                                  user.EmailConfirmed,
                                                  user.Role);
    }
}
