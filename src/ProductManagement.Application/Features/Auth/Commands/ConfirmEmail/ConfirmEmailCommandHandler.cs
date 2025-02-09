using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.ConfirmEmail;

/// <summary>
/// Handles the <see cref="ConfirmEmailCommand"/> by verifying the user's email confirmation token 
/// and updating their authentication status.
/// </summary>
/// /// <remarks>
/// The <see cref="Handle"/> method will return a Success Result if the user is not found to obfuscate from attackers.
/// </remarks>
internal sealed class ConfirmEmailCommandHandler : ICommandHandler<ConfirmEmailCommand>
{
    private readonly ILogger<ConfirmEmailCommandHandler> _logger;
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public ConfirmEmailCommandHandler(ILogger<ConfirmEmailCommandHandler> logger, IAuthService authService, IUserService userService)
    {
        _logger = logger;
        _authService = authService;
        _userService = userService;
    }

    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Starting {handler} for UserId {userId}", nameof(ConfirmEmailCommandHandler), request.UserId);

        var userResult = await _userService.FindByIdAsync(request.UserId, cancellationToken);
        if (userResult.IsFailure)
        {
            _logger.LogWarning("UserId {userId}: {errorCode} - {errorMessage}", request.UserId, userResult.Error.Code, userResult.Error.Message);
            return Result.Success();
        }

        var confirmEmailResult = await _authService.ConfirmEmailAsync(request.UserId, request.Token, cancellationToken);
        if (confirmEmailResult.IsFailure)
        {
            _logger.LogWarning("UserId {userId}: {errorCode} - {errorMessage}", request.UserId, confirmEmailResult.Error.Code, confirmEmailResult.Error.Message);
            return Result.Failure(confirmEmailResult.Error);
        }

        var signInResult = await _authService.RefreshSignInAsync(request.UserId, cancellationToken);
        if (signInResult.IsFailure)
        {
            _logger.LogWarning("UserId {userId}: {errorCode} - {errorMessage}", request.UserId, signInResult.Error.Code, signInResult.Error.Message);
            return Result.Failure(signInResult.Error);
        }

        _logger.LogInformation("Finished {handler} for UserId {userId} successfully", nameof(ConfirmEmailCommandHandler), request.UserId);
        return Result.Success();
    }
}
