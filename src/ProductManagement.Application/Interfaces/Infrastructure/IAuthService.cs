using System.Security.Claims;
using ProductManagement.Application.Models;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Interfaces.Infrastructure;

public interface IAuthService
{
    Task<Result> AddToRoleAsync(string email, string? role, CancellationToken cancellationToken = default);
    Task<Result> ChangePasswordAsync(ClaimsPrincipal principal, string currentPassword, string newPassword, CancellationToken cancellationToken = default);
    Task<Result> ConfirmEmailAsync(string userId, AuthToken token, CancellationToken cancellationToken = default);
    Task<Result> ForgotPasswordAsync(string email, string resetUrl, CancellationToken cancellationToken = default); // TODO: Split into GeneratePasswordResetTokenAsync and SendPasswordResetEmailAsync.
    Task<Result> GenerateEmailChangeAsync(ClaimsPrincipal principal, string email, string confirmUrl, CancellationToken cancellationToken = default);
    Task<Result> GenerateEmailConfirmationAsync(string email, string confirmUrl, CancellationToken cancellationToken = default);
    Task<Result<ApplicationUserDto>> GetCurrentUserAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default);
    Task<Result> PasswordSignInAsync(string email, string password, bool remember, CancellationToken cancellationToken = default);
    Task<Result> RefreshSignInAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result> RegisterAsync(string email, string? password, CancellationToken cancellationToken = default);
    Task<Result> ResetPasswordAsync(string email, string password, AuthToken token, CancellationToken cancellationToken = default);
    Task<Result> SignInAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result> SignOutAsync(CancellationToken cancellationToken = default);
}
