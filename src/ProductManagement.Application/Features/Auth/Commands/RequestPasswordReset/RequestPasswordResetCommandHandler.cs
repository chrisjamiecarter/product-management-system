using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.RequestPasswordReset;

/// <summary>
/// Handles the <see cref="RequestPasswordResetCommand"/> by generating a password reset token, 
/// building a reset link, and sending it via email.
/// </summary>
/// <remarks>
/// The <see cref="Handle"/> method will return a Success Result if the user is not found to obfuscate from attackers.
/// </remarks>
internal sealed class RequestPasswordResetCommandHandler : ICommandHandler<RequestPasswordResetCommand>
{
    private readonly ILogger<RequestPasswordResetCommandHandler> _logger;
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    private readonly ILinkBuilderService _linkBuilderService;
    private readonly IUserService _userService;

    public RequestPasswordResetCommandHandler(ILogger<RequestPasswordResetCommandHandler> logger, IAuthService authService, IEmailService emailService, ILinkBuilderService linkBuilderService, IUserService userService)
    {
        _logger = logger;
        _authService = authService;
        _emailService = emailService;
        _linkBuilderService = linkBuilderService;
        _userService = userService;
    }

    public async Task<Result> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken = default)
    {
        var userResult = await _userService.FindByEmailAsync(request.Email, cancellationToken);
        if (userResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", userResult.Error);
            return Result.Success();
        }

        var user = userResult.Value;

        var tokenResult = await _authService.GeneratePasswordResetTokenAsync(request.Email, cancellationToken);
        if (tokenResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", tokenResult.Error);
            return Result.Failure(tokenResult.Error);
        }

        var passwordResetLink = await _linkBuilderService.BuildPasswordResetLinkAsync(tokenResult.Value, cancellationToken);

        var emailResult = await _emailService.SendPasswordResetAsync(request.Email, passwordResetLink, cancellationToken);
        if (emailResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", emailResult.Error);
            return Result.Failure(emailResult.Error);
        }

        _logger.LogInformation("Sent reset password link for User {id} successfully", user.Id);
        return Result.Success();
    }
}
