using ProductManagement.Application.Models;

namespace ProductManagement.Application.Repositories;

public interface IUserRepository
{
    Task<PaginatedList<User>> ReturnByPageAsync(string? searchUserName,
                                                bool? searchEmailConfirmed,
                                                string? sortOrder,
                                                int pageNumber,
                                                int pageSize,
                                                CancellationToken cancellationToken = default);
}
