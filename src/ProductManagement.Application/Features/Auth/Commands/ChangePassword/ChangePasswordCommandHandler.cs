using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.ChangePassword;

/// <summary>
/// Handles the <see cref="ChangePasswordCommand"/> by validating the user, updating the password, 
/// and refreshing the authentication session.
/// </summary>
/// /// <remarks>
/// The <see cref="Handle"/> method will return a Success Result if the user is not found to obfuscate from attackers.
/// </remarks>
internal sealed class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
{
    private readonly ILogger<ChangePasswordCommandHandler> _logger;
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public ChangePasswordCommandHandler(ILogger<ChangePasswordCommandHandler> logger, IAuthService authService, IUserService userService)
    {
        _logger = logger;
        _authService = authService;
        _userService = userService;
    }

    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Starting {handler} for UserId {userId}",
            nameof(ChangePasswordCommandHandler), request.UserId);

        var userResult = await _userService.FindByIdAsync(request.UserId, cancellationToken);
        if (userResult.IsFailure)
        {
            _logger.LogWarning("Failure during {handler} for UserId {userId}. {errorCode} - {errorMessage}",
                nameof(ChangePasswordCommandHandler), request.UserId, userResult.Error.Code, userResult.Error.Message);

            // Obfuscate user not found.
            return Result.Success();
        }

        var passwordResult = await _userService.ChangePasswordAsync(request.UserId, request.CurrentPassword, request.UpdatedPassword, cancellationToken);
        if (passwordResult.IsFailure)
        {
            _logger.LogWarning("Failure during {handler} for UserId {userId}. {errorCode} - {errorMessage}",
                nameof(ChangePasswordCommandHandler), request.UserId, passwordResult.Error.Code, passwordResult.Error.Message);
            
            return Result.Failure(passwordResult.Error);
        }

        var refreshResult = await _authService.RefreshSignInAsync(request.UserId, cancellationToken);
        if (refreshResult.IsFailure)
        {
            _logger.LogWarning("Failure during {handler} for UserId {userId}. {errorCode} - {errorMessage}",
                nameof(ChangePasswordCommandHandler), request.UserId, refreshResult.Error.Code, refreshResult.Error.Message);
            
            return Result.Failure(refreshResult.Error);
        }

        _logger.LogInformation("Finished {handler} for UserId {userId} successfully",
            nameof(ChangePasswordCommandHandler), request.UserId);

        return Result.Success();
    }
}
