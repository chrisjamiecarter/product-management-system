using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.GenerateEmailChange;

/// <summary>
/// Handles the <see cref="GenerateEmailChangeCommand"/> by validating the user, ensuring the new email 
/// is not already taken, generating an email change token, and sending a confirmation email.
/// </summary>
/// <remarks>
/// The <see cref="Handle"/> method will return a Success Result if the user is not found to obfuscate from attackers.
/// </remarks>
internal sealed class GenerateEmailChangeCommandHandler : ICommandHandler<GenerateEmailChangeCommand>
{
    private readonly ILogger<GenerateEmailChangeCommandHandler> _logger;
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    private readonly ILinkBuilderService _linkBuilderService;
    private readonly IUserService _userService;

    public GenerateEmailChangeCommandHandler(ILogger<GenerateEmailChangeCommandHandler> logger, IAuthService authService, IEmailService emailService, ILinkBuilderService linkBuilderService, IUserService userService)
    {
        _logger = logger;
        _authService = authService;
        _emailService = emailService;
        _linkBuilderService = linkBuilderService;
        _userService = userService;
    }

    public async Task<Result> Handle(GenerateEmailChangeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Starting {handler} for UserId {userId}", nameof(GenerateEmailChangeCommandHandler), request.UserId);

        var userResult = await _userService.FindByIdAsync(request.UserId, cancellationToken);
        if (userResult.IsFailure)
        {
            _logger.LogWarning("UserId {userId}: {errorCode} - {errorMessage}", request.UserId, userResult.Error.Code, userResult.Error.Message);
            return Result.Success();
        }

        var duplicateEmailResult = await _userService.FindByEmailAsync(request.UpdatedEmail, cancellationToken);
        if (duplicateEmailResult.IsSuccess)
        {
            _logger.LogWarning("UserId {userId}: {errorCode} - {errorMessage}", request.UserId, duplicateEmailResult.Error.Code, duplicateEmailResult.Error.Message);
            return Result.Failure(ApplicationErrors.User.EmailTaken);
        }

        var tokenResult = await _authService.GenerateEmailChangeTokenAsync(request.UserId, request.UpdatedEmail, cancellationToken);
        if (tokenResult.IsFailure)
        {
            _logger.LogWarning("UserId {userId}: {errorCode} - {errorMessage}", request.UserId, tokenResult.Error.Code, tokenResult.Error.Message);
            return Result.Failure(tokenResult.Error);
        }

        var changeEmailConfirmationLink = await _linkBuilderService.BuildChangeEmailConfirmationLinkAsync(request.UserId, request.UpdatedEmail, tokenResult.Value, cancellationToken);

        var emailResult = await _emailService.SendChangeEmailConfirmationAsync(request.UpdatedEmail, changeEmailConfirmationLink, cancellationToken);
        if (emailResult.IsFailure)
        {
            _logger.LogWarning("UserId {userId}: {errorCode} - {errorMessage}", request.UserId, emailResult.Error.Code, emailResult.Error.Message);
            return Result.Failure(tokenResult.Error);
        }

        _logger.LogInformation("Finished {handler} for UserId {userId} successfully", nameof(GenerateEmailChangeCommandHandler), request.UserId);
        return Result.Success();
    }
}
