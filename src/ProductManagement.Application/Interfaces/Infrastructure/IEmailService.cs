namespace ProductManagement.Application.Interfaces.Infrastructure;

public interface IEmailService
{
    Task SendChangeEmailConfirmationAsync(string toEmailAddress, string changeEmailConfirmationLink, CancellationToken cancellationToken = default);
    Task SendEmailConfirmationAsync(string toEmailAddress, string emailConfirmationLink, CancellationToken cancellationToken = default);
    Task SendPasswordResetAsync(string toEmailAddress, string passwordResetLink, CancellationToken cancellationToken = default);
}
