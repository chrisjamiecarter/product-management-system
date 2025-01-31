using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Infrastructure.EmailRender.Interfaces;
using ProductManagement.Infrastructure.EmailRender.Views.Emails.EmailConfirmation;
using ProductManagement.Infrastructure.EmailRender.Views.Emails.PasswordReset;
using ProductManagement.Infrastructure.Options;

namespace ProductManagement.Infrastructure.Services;

internal class EmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;
    private readonly IRazorViewToStringRenderService _renderService;

    public EmailService(IOptions<EmailOptions> emailOptions, IRazorViewToStringRenderService renderService)
    {
        _emailOptions = emailOptions.Value;
        _renderService = renderService;
    }

    public async Task SendChangeEmailConfirmationAsync(string toEmailAddress, string changeEmailConfirmationLink, CancellationToken cancellationToken = default)
    {
        var body = $"Please confirm your account by <a href='{changeEmailConfirmationLink}'>clicking here</a>.";

        await SendEmailAsync(toEmailAddress, "Confirm your email", body, cancellationToken);
    }

    public async Task SendEmailConfirmationAsync(string toEmailAddress, string emailConfirmationLink, CancellationToken cancellationToken = default)
    {
        var emailConfirmationViewModel = new EmailConfirmationViewModel(emailConfirmationLink);
        var body = await _renderService.RenderViewToStringAsync("/Views/Emails/EmailConfirmation/EmailConfirmation.cshtml", emailConfirmationViewModel);

        await SendEmailAsync(toEmailAddress, "Confirm your email", body, cancellationToken);
    }

    public async Task SendPasswordResetAsync(string toEmailAddress, string passwordResetLink, CancellationToken cancellationToken = default)
    {
        var passwordResetViewModel = new PasswordResetViewModel(passwordResetLink);
        var body = await _renderService.RenderViewToStringAsync("/Views/Emails/PasswordReset/PasswordReset.cshtml", passwordResetViewModel);

        await SendEmailAsync(toEmailAddress, "Reset your password", body, cancellationToken);
    }

    private async Task SendEmailAsync(string toEmailAddress, string subject, string body, CancellationToken cancellationToken)
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

}
