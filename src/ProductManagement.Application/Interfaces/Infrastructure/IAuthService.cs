using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Interfaces.Infrastructure;

public interface IAuthService
{
    Task<Result> RegisterUserAsync(string email, string password, string confirmUrl, string returnUrl, CancellationToken cancellationToken = default);
    Task<Result> ForgotPasswordAsync(string email, string resetUrl, CancellationToken cancellationToken = default);
    Task<Result> SignInUserAsync(string email, string password, bool remember, CancellationToken cancellationToken = default);
}
