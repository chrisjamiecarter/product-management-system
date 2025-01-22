namespace ProductManagement.Application.Services;

public interface IEmailService
{
    Task SendEmailAsync(string toEmailAddress, string subject, string body, CancellationToken cancellationToken);
}
