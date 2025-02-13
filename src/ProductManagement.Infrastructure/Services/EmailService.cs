using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;
using ProductManagement.Infrastructure.EmailRender.Interfaces;
using ProductManagement.Infrastructure.EmailRender.Views.Emails.ChangeEmailConfirmation;
using ProductManagement.Infrastructure.EmailRender.Views.Emails.EmailConfirmation;
using ProductManagement.Infrastructure.EmailRender.Views.Emails.PasswordReset;
using ProductManagement.Infrastructure.Options;

namespace ProductManagement.Infrastructure.Services;

/// <summary>
/// Provides the service for email operations.
/// </summary>
internal class EmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;
    private readonly ILogger<EmailService> _logger;
    private readonly IRazorViewToStringRenderService _renderService;

    public EmailService(ILogger<EmailService> logger, IOptions<EmailOptions> emailOptions, IRazorViewToStringRenderService renderService)
    {
        _emailOptions = emailOptions.Value;
        _logger = logger;
        _renderService = renderService;
    }

    public async Task<Result> SendChangeEmailConfirmationAsync(string toEmailAddress, string changeEmailConfirmationLink, CancellationToken cancellationToken = default)
    {
        var changeEmailConfirmationViewModel = new ChangeEmailConfirmationViewModel(changeEmailConfirmationLink);
        var body = await _renderService.RenderViewToStringAsync("/Views/Emails/ChangeEmailConfirmation/ChangeEmailConfirmation.cshtml", changeEmailConfirmationViewModel);

        return await SendEmailAsync(toEmailAddress, "Confirm your change of email", body, cancellationToken);
    }

    public async Task<Result> SendEmailConfirmationAsync(string toEmailAddress, string emailConfirmationLink, CancellationToken cancellationToken = default)
    {
        var emailConfirmationViewModel = new EmailConfirmationViewModel(emailConfirmationLink);
        var body = await _renderService.RenderViewToStringAsync("/Views/Emails/EmailConfirmation/EmailConfirmation.cshtml", emailConfirmationViewModel);

        return await SendEmailAsync(toEmailAddress, "Confirm your email", body, cancellationToken);
    }

    public async Task<Result> SendPasswordResetAsync(string toEmailAddress, string passwordResetLink, CancellationToken cancellationToken = default)
    {
        var passwordResetViewModel = new PasswordResetViewModel(passwordResetLink);
        var body = await _renderService.RenderViewToStringAsync("/Views/Emails/PasswordReset/PasswordReset.cshtml", passwordResetViewModel);

        return await SendEmailAsync(toEmailAddress, "Reset your password", body, cancellationToken);
    }

    private async Task<Result> SendEmailAsync(string toEmailAddress, string subject, string body, CancellationToken cancellationToken)
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

            return Result.Success();
        }
        catch (Exception exception)
        {
            _logger.LogWarning("{Exception}", exception.Message);
            return Result.Failure(ApplicationErrors.Email.NotSent(toEmailAddress));
        }
        finally
        {
            await client.DisconnectAsync(true, cancellationToken);
        }
    }
}
