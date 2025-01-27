using System.Security.Claims;
using ProductManagement.Application.Models;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Interfaces.Infrastructure;

public interface IAuthService
{
    Task<Result> ChangePasswordAsync(ClaimsPrincipal principal, string currentPassword, string newPassword, CancellationToken cancellationToken = default);
    Task<Result> ConfirmEmailAsync(string userId, string token, CancellationToken cancellationToken = default);
    Task<Result> ConfirmEmailChangeAsync(string userId, string email, string token, CancellationToken cancellationToken = default);
    Task<Result> ForgotPasswordAsync(string email, string resetUrl, CancellationToken cancellationToken = default);
    Task<Result> GenerateEmailChangeAsync(ClaimsPrincipal principal, string email, string confirmUrl, CancellationToken cancellationToken = default);
    Task<Result> GenerateEmailConfirmationAsync(string email, string confirmUrl, string returnUrl, CancellationToken cancellationToken = default);
    Task<Result<ApplicationUserDto>> GetCurrentUserAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default);
    Task<Result> RegisterAsync(string email, string password, string confirmUrl, string returnUrl, CancellationToken cancellationToken = default);
    Task<Result> ResetPasswordAsync(string email, string password, string token, CancellationToken cancellationToken = default);
    Task<Result> SignInAsync(string email, string password, bool remember, CancellationToken cancellationToken = default);
    Task<Result> SignOutAsync(CancellationToken cancellationToken = default);
}
