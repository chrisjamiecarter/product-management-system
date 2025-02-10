using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.RequestEmailChangeConfirmation;

/// <summary>
/// Handles the <see cref="RequestEmailChangeConfirmationCommand"/> by validating the user, ensuring the new email 
/// is not already taken, generating an email change token, and sending a confirmation email.
/// </summary>
/// <remarks>
/// The <see cref="Handle"/> method will return a Success Result if the user is not found to obfuscate from attackers.
/// </remarks>
internal sealed class RequestEmailChangeConfirmationCommandHandler : ICommandHandler<RequestEmailChangeConfirmationCommand>
{
    private readonly ILogger<RequestEmailChangeConfirmationCommandHandler> _logger;
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    private readonly ILinkBuilderService _linkBuilderService;
    private readonly IUserService _userService;

    public RequestEmailChangeConfirmationCommandHandler(ILogger<RequestEmailChangeConfirmationCommandHandler> logger, IAuthService authService, IEmailService emailService, ILinkBuilderService linkBuilderService, IUserService userService)
    {
        _logger = logger;
        _authService = authService;
        _emailService = emailService;
        _linkBuilderService = linkBuilderService;
        _userService = userService;
    }

    public async Task<Result> Handle(RequestEmailChangeConfirmationCommand request, CancellationToken cancellationToken)
    {
        var userResult = await _userService.FindByIdAsync(request.UserId, cancellationToken);
        if (userResult.IsFailure)
        {
            _logger.LogWarning("UserId {userId}: {errorCode} - {errorMessage}", request.UserId, userResult.Error.Code, userResult.Error.Message);
            return Result.Success();
        }

        var duplicateEmailResult = await _userService.FindByEmailAsync(request.UpdatedEmail, cancellationToken);
        if (duplicateEmailResult.IsSuccess)
        {
            var error = ApplicationErrors.User.EmailTaken;
            _logger.LogWarning("UserId {userId}: {errorCode} - {errorMessage}", request.UserId, error.Code, error.Message);
            return Result.Failure(error);
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

        _logger.LogInformation("Sent confirm email change link for User {id} successfully", request.UserId);
        return Result.Success();
    }
}
