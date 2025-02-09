using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.ForgotPassword;

/// <summary>
/// Handles the <see cref="ForgotPasswordCommand"/> by generating a password reset token, 
/// building a reset link, and sending it via email.
/// </summary>
/// <remarks>
/// The <see cref="Handle"/> method will return a Success Result if the user is not found to obfuscate from attackers.
/// </remarks>
internal sealed class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand>
{
    private readonly ILogger<ForgotPasswordCommandHandler> _logger;
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    private readonly ILinkBuilderService _linkBuilderService;
    private readonly IUserService _userService;

    public ForgotPasswordCommandHandler(ILogger<ForgotPasswordCommandHandler> logger, IAuthService authService, IEmailService emailService, ILinkBuilderService linkBuilderService, IUserService userService)
    {
        _logger = logger;
        _authService = authService;
        _emailService = emailService;
        _linkBuilderService = linkBuilderService;
        _userService = userService;
    }

    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Starting {handler} for Email {email}", nameof(ForgotPasswordCommandHandler), request.Email);

        var userResult = await _userService.FindByEmailAsync(request.Email, cancellationToken);
        if (userResult.IsFailure)
        {
            _logger.LogWarning("Email {email}: {errorCode} - {errorMessage}", request.Email, userResult.Error.Code, userResult.Error.Message);
            return Result.Success();
        }

        var tokenResult = await _authService.GeneratePasswordResetTokenAsync(request.Email, cancellationToken);
        if (tokenResult.IsFailure)
        {
            _logger.LogWarning("Email {email}: {errorCode} - {errorMessage}", request.Email, tokenResult.Error.Code, tokenResult.Error.Message);
            return Result.Failure(tokenResult.Error);
        }

        var passwordResetLink = await _linkBuilderService.BuildPasswordResetLinkAsync(tokenResult.Value, cancellationToken);

        var emailResult = await _emailService.SendPasswordResetAsync(request.Email, passwordResetLink, cancellationToken);
        if (emailResult.IsFailure)
        {
            _logger.LogWarning("Email {email}: {errorCode} - {errorMessage}", request.Email, emailResult.Error.Code, emailResult.Error.Message);
            return Result.Failure(emailResult.Error);
        }

        _logger.LogInformation("Finished {handler} for Email {email} successfully", nameof(ForgotPasswordCommandHandler), request.Email);
        return Result.Success();
    }
}
