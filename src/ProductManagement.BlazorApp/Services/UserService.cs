using MediatR;
using ProductManagement.Application.Models;
using ProductManagement.Application.Services;
using ProductManagement.Application.Users.Queries.GetUsersPaginated;
using ProductManagement.Domain.Shared;

namespace ProductManagement.BlazorApp.Services;

public class UserService : IUserService
{
    private readonly ISender _sender;

    public UserService(ISender sender)
    {
        _sender = sender;
    }

    public async Task<Result<PaginatedList<GetUsersPaginatedQueryResponse>>> ReturnByPageAsync(GetUsersPaginatedQuery query, CancellationToken cancellationToken = default)
    {
        return await _sender.Send(query, cancellationToken);
    }
}
