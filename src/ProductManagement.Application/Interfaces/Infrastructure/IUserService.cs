using ProductManagement.Application.Models;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Interfaces.Infrastructure;

public interface IUserService
{
    Task<Result> ChangeEmailAsync(string userId, string email, AuthToken token, CancellationToken cancellationToken = default);
    Task<Result<ApplicationUserDto>> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Result<ApplicationUserDto>> FindByIdAsync(string userId, CancellationToken cancellationToken = default);
}
