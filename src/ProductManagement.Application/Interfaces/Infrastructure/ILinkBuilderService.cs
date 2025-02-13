using ProductManagement.Application.Models;

namespace ProductManagement.Application.Interfaces.Infrastructure;

/// <summary>
/// Defines the service for link building operations.
/// </summary>
public interface ILinkBuilderService
{
    Task<string> BuildChangeEmailConfirmationLinkAsync(string userId, string email, AuthToken token, CancellationToken cancellationToken = default);
    Task<string> BuildEmailConfirmationLinkAsync(string userId, AuthToken token, CancellationToken cancellationToken = default);
    Task<string> BuildPasswordResetLinkAsync(AuthToken token, CancellationToken cancellationToken = default);
}
