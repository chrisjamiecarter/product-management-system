using ProductManagement.Application.Models;
using ProductManagement.Application.Users.Queries.GetUserById;
using ProductManagement.Application.Users.Queries.GetUsersPaginated;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Services;

public interface IUserService
{
    Task<Result> CreateAsync(string userName, string role, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result<GetUserByIdQueryResponse>> ReturnByIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result<PaginatedList<GetUsersPaginatedQueryResponse>>> ReturnByPageAsync(string? searchUsername, string? searchRole, bool? searchEmailConfirmed, string? sortColumn, string? sortOrder, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(string userId, string userName, string role, bool emailConfirmed, CancellationToken cancellationToken = default);
}
