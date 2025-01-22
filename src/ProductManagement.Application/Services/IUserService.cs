using ProductManagement.Application.Models;
using ProductManagement.Application.Users.Commands.CreateUser;
using ProductManagement.Application.Users.Commands.DeleteUser;
using ProductManagement.Application.Users.Commands.UpdateUser;
using ProductManagement.Application.Users.Queries.GetUserById;
using ProductManagement.Application.Users.Queries.GetUsersPaginated;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Services;

public interface IUserService
{
    Task<Result> CreateAsync(CreateUserCommand command, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(DeleteUserCommand command, CancellationToken cancellationToken = default);
    Task<Result<GetUserByIdQueryResponse>> ReturnByIdAsync(GetUserByIdQuery query, CancellationToken cancellationToken = default);
    Task<Result<PaginatedList<GetUsersPaginatedQueryResponse>>> ReturnByPageAsync(GetUsersPaginatedQuery query, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(UpdateUserCommand command, CancellationToken cancellationToken = default);
}
