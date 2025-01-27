using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Infrastructure.Database.Models;
using ProductManagement.Infrastructure.Email.Options;

namespace ProductManagement.Infrastructure.Email.Services;

internal class EmailService(IOptions<EmailOptions> emailOptions) : IEmailService, IEmailSender<ApplicationUser>
{
    private readonly EmailOptions _emailOptions = emailOptions.Value;

    public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
    {
        await SendEmailAsync(email, "Confirm your email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.", CancellationToken.None);
    }

    public async Task SendEmailAsync(string toEmailAddress, string subject, string body, CancellationToken cancellationToken = default)
    {
        // TODO: Errors?

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

    public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
    {
        await SendEmailAsync(email, "Reset your password", $"Please reset your password using the following code: {resetCode}", CancellationToken.None);
    }

    public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
    {
        await SendEmailAsync(email, "Reset your password", $"Please reset your password by <a href='{resetLink}'>clicking here</a>.", CancellationToken.None);
    }
}
