using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Infrastructure.Email.Options;

namespace ProductManagement.Infrastructure.Email.Services;

internal class EmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;

    public EmailService(IOptions<EmailOptions> emailOptions)
    {
        _emailOptions = emailOptions.Value;
    }

    public async Task SendEmailAsync(string toEmailAddress, string subject, string body, CancellationToken cancellationToken = default)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_emailOptions.FromName, _emailOptions.FromEmailAddress));
        email.To.Add(new MailboxAddress(toEmailAddress, toEmailAddress));
        email.Subject = subject;

        email.Body = new TextPart("html")
        {
            Text = body
        };

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_emailOptions.SmtpHost, _emailOptions.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken);
            await client.AuthenticateAsync(_emailOptions.SmtpUser, _emailOptions.SmtpPassword, cancellationToken);

            await client.SendAsync(email, cancellationToken);
        }
        catch (Exception exception)
        {
            // TODO: Log.
            Console.WriteLine(exception.Message);
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true, cancellationToken);
        }
    }
}
