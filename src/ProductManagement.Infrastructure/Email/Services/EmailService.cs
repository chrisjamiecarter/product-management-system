using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Infrastructure.Database.Models;
using ProductManagement.Infrastructure.Email.Options;
using ProductManagement.Infrastructure.EmailRender.Interfaces;
using ProductManagement.Infrastructure.EmailRender.Views.Emails.PasswordReset;

namespace ProductManagement.Infrastructure.Email.Services;

internal class EmailService : IEmailService, IEmailSender<ApplicationUser>
{
    private readonly EmailOptions _emailOptions;
    private readonly IRazorViewToStringRenderService _renderService;

    public EmailService(IOptions<EmailOptions> emailOptions, IRazorViewToStringRenderService renderService)
    {
        _emailOptions = emailOptions.Value;
        _renderService = renderService;
    }

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
        var passwordResetViewModel = new PasswordResetViewModel(resetLink);
        var body = await _renderService.RenderViewToStringAsync("/Views/Emails/PasswordReset/PasswordReset.cshtml", passwordResetViewModel);
        await SendEmailAsync(email, "Reset your password", body, CancellationToken.None);
    }
}
