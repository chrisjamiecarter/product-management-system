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
        var userResult = await _userService.FindByIdAsync(request.UserId, cancellationToken);
        if (userResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", userResult.Error);
            return Result.Success();
        }

        var confirmEmailResult = await _authService.ConfirmEmailAsync(request.UserId, request.Token, cancellationToken);
        if (confirmEmailResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", confirmEmailResult.Error);
            return Result.Failure(confirmEmailResult.Error);
        }

        var signInResult = await _authService.RefreshSignInAsync(request.UserId, cancellationToken);
        if (signInResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", signInResult.Error);
            return Result.Failure(signInResult.Error);
        }

        _logger.LogInformation("Confirmed email for User {id} successfully", request.UserId);
        return Result.Success();
    }
}
