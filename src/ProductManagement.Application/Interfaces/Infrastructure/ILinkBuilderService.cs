using ProductManagement.Application.Models;

namespace ProductManagement.Application.Interfaces.Infrastructure;

public interface ILinkBuilderService
{
    Task<string> BuildChangeEmailConfirmationLinkAsync(string userId, string email, AuthToken token, CancellationToken cancellationToken = default);
    Task<string> BuildEmailConfirmationLinkAsync(string userId, AuthToken token, CancellationToken cancellationToken = default);
    Task<string> BuildPasswordResetLinkAsync(AuthToken token, CancellationToken cancellationToken = default);
}
