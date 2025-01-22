using MediatR;
using ProductManagement.Application.Models;
using ProductManagement.Application.Services;
using ProductManagement.Application.Users.Commands.CreateUser;
using ProductManagement.Application.Users.Commands.DeleteUser;
using ProductManagement.Application.Users.Commands.UpdateUser;
using ProductManagement.Application.Users.Queries.GetUserById;
using ProductManagement.Application.Users.Queries.GetUsersPaginated;
using ProductManagement.Domain.Shared;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ProductManagement.BlazorApp.Services;

public class UserService : IUserService
{
    private readonly ISender _sender;

    public UserService(ISender sender)
    {
        _sender = sender;
    }

    public async Task<Result> CreateAsync(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        return await _sender.Send(command, cancellationToken);
    }

    public async Task<Result> DeleteAsync(DeleteUserCommand command, CancellationToken cancellationToken = default)
    {
        return await _sender.Send(command, cancellationToken);
    }

    public async Task<Result<GetUserByIdQueryResponse>> ReturnByIdAsync(GetUserByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _sender.Send(query, cancellationToken);
    }

    public async Task<Result<PaginatedList<GetUsersPaginatedQueryResponse>>> ReturnByPageAsync(GetUsersPaginatedQuery query, CancellationToken cancellationToken = default)
    {
        return await _sender.Send(query, cancellationToken);
    }

    public async Task<Result> UpdateAsync(UpdateUserCommand command, CancellationToken cancellationToken = default)
    {
        return await _sender.Send(command, cancellationToken);
    }
}
