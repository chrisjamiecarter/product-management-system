namespace ProductManagement.Application.Interfaces.Infrastructure;

public interface IEmailService
{
    Task SendEmailAsync(string toEmailAddress, string subject, string body, CancellationToken cancellationToken = default);
}
