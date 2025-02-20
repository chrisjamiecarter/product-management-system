using ProductManagement.Application.Models;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Interfaces.Infrastructure;

/// <summary>
/// Defines the service for user operations.
/// </summary>
public interface IUserService
{
    Task<Result> AddPasswordAsync(string userId, string password, CancellationToken cancellationToken = default);
    Task<Result> ChangeEmailAsync(string userId, string updatedEmail, AuthToken token, CancellationToken cancellationToken = default);
    Task<Result> ChangePasswordAsync(string userId, string currentPassword, string updatedPassword, CancellationToken cancellationToken = default);
    Task<Result> CreateAsync(string email, bool emailConfirmed, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result<ApplicationUserDto>> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Result<ApplicationUserDto>> FindByIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result<ApplicationUserDto>> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = default);
    Task<Result<PaginatedList<ApplicationUserDto>>> GetPageAsync(string? searchEmail, bool? searchEmailConfirmed, string? searchRole, string? sortColumn, string? sortOrder, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<Result<bool>> HasPasswordAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result<bool>> IsEmailConfirmedAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result> RemoveExternalLoginAsync(string userId, string provider, string providerKey, CancellationToken cancellationToken = default);
    Task<Result> UpdateRoleAsync(string userId, string? role, CancellationToken cancellationToken = default);
}
