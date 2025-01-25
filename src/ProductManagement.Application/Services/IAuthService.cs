using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Services;

public interface IAuthService
{
    Task<Result> RegisterUserAsync(string email, string password, string confirmUrl, string returnUrl, CancellationToken cancellationToken = default);
    Task<Result> SignInUserAsync(string email, string password, bool remember, CancellationToken cancellationToken = default);
}
