using MediatR;
using ProductManagement.Application.Models;
using ProductManagement.Application.Services;
using ProductManagement.Application.Users.Commands.CreateUser;
using ProductManagement.Application.Users.Commands.DeleteUser;
using ProductManagement.Application.Users.Commands.UpdateUser;
using ProductManagement.Application.Users.Queries.GetUserById;
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

    public async Task<Result> CreateAsync(string userName, string role, CancellationToken cancellationToken = default)
    {
        var command = new CreateUserCommand(userName, role);
        return await _sender.Send(command, cancellationToken);
    }

    public async Task<Result> DeleteAsync(string userId, CancellationToken cancellationToken = default)
    {
        var command = new DeleteUserCommand(userId);
        return await _sender.Send(command, cancellationToken);
    }

    public async Task<Result<GetUserByIdQueryResponse>> ReturnByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var query = new GetUserByIdQuery(userId);
        return await _sender.Send(query, cancellationToken);
    }

    public async Task<Result<PaginatedList<GetUsersPaginatedQueryResponse>>> ReturnByPageAsync(string? searchUsername, string? searchRole, bool? searchEmailConfirmed, string? sortColumn, string? sortOrder, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var query = new GetUsersPaginatedQuery(searchUsername, searchRole, searchEmailConfirmed, sortColumn, sortOrder, pageNumber, pageSize);
        return await _sender.Send(query, cancellationToken);
    }

    public async Task<Result> UpdateAsync(string userId, string userName, string role, bool emailConfirmed, CancellationToken cancellationToken = default)
    {
        var command = new UpdateUserCommand(userId, userName, role, emailConfirmed);
        return await _sender.Send(command, cancellationToken);
    }
}
