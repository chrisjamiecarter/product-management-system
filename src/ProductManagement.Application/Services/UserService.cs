using MediatR;
using ProductManagement.Application.Features.Users.Commands.CreateUser;
using ProductManagement.Application.Features.Users.Commands.DeleteUser;
using ProductManagement.Application.Features.Users.Commands.UpdateUser;
using ProductManagement.Application.Features.Users.Queries.GetUserByEmail;
using ProductManagement.Application.Features.Users.Queries.GetUserById;
using ProductManagement.Application.Features.Users.Queries.GetUsersPaginated;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Application.Models;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Services;

// TODO: Move to Infrastructure layer.
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

    public async Task<Result<ApplicationUserDto>> ReturnByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var query = new GetUserByEmailQuery(email);
        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Result.Success(result.Value.ToDto())
            : Result.Failure<ApplicationUserDto>(result.Error);
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
