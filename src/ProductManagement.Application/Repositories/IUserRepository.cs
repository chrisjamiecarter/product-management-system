using ProductManagement.Application.Models;

namespace ProductManagement.Application.Repositories;

public interface IUserRepository
{
    Task<bool> CreateAsync(string username, CancellationToken cancellationToken = default);
    Task<User?> ReturnByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<PaginatedList<User>> ReturnByPageAsync(string? searchUsername,
                                                bool? searchEmailConfirmed,
                                                string? sortOrder,
                                                int pageNumber,
                                                int pageSize,
                                                CancellationToken cancellationToken = default);
    Task<User?> ReturnByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(User user, CancellationToken cancellationToken = default);
}
