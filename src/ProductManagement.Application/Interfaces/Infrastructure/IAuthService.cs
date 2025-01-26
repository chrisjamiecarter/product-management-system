using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Interfaces.Infrastructure;

public interface IAuthService
{
    Task<Result> ConfirmEmailAsync(string userId, string token, CancellationToken cancellationToken = default);
    Task<Result> RegisterAsync(string email, string password, string confirmUrl, string returnUrl, CancellationToken cancellationToken = default);
    Task<Result> ForgotPasswordAsync(string email, string resetUrl, CancellationToken cancellationToken = default);
    Task<Result> SignInAsync(string email, string password, bool remember, CancellationToken cancellationToken = default);
}
