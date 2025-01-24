using ProductManagement.Application.Models;

namespace ProductManagement.Application.Repositories;

public interface IUserRepository
{
    Task<bool> CreateAsync(string userName, string role, CancellationToken cancellationToken = default);
    Task<ApplicationUserDto?> ReturnByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<PaginatedList<ApplicationUserDto>> ReturnByPageAsync(string? searchUsername,
                                                              string? searchRole,
                                                              bool? searchEmailConfirmed,
                                                              string? sortColumn,
                                                              string? sortOrder,
                                                              int pageNumber,
                                                              int pageSize,
                                                              CancellationToken cancellationToken = default);
    Task<ApplicationUserDto?> ReturnByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(ApplicationUserDto user, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(ApplicationUserDto user, CancellationToken cancellationToken = default);
}
