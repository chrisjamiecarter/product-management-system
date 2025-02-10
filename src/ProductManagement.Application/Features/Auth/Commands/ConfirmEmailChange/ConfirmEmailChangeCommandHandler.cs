using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.ConfirmEmailChange;

/// <summary>
/// Handles the <see cref="ConfirmEmailChangeCommand"/> to confirm an email change for a user. 
/// This involves finding the user, changing their email and username, 
/// and refreshing their sign-in session.
/// </summary>
/// <remarks>
/// The <see cref="Handle"/> method will return a Success Result if the user is not found to obfuscate from attackers.
/// </remarks>
internal sealed class ConfirmEmailChangeCommandHandler : ICommandHandler<ConfirmEmailChangeCommand>
{
    private readonly ILogger<ConfirmEmailChangeCommandHandler> _logger;
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public ConfirmEmailChangeCommandHandler(ILogger<ConfirmEmailChangeCommandHandler> logger, IAuthService authService, IUserService userService)
    {
        _logger = logger;
        _authService = authService;
        _userService = userService;
    }

    public async Task<Result> Handle(ConfirmEmailChangeCommand request, CancellationToken cancellationToken)
    {
        var userResult = await _userService.FindByIdAsync(request.UserId, cancellationToken);
        if (userResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", userResult.Error);
            return Result.Success();
        }

        var emailResult = await _userService.ChangeEmailAsync(request.UserId, request.Email, request.Token, cancellationToken);
        if (emailResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", emailResult.Error);
            return Result.Failure(emailResult.Error);
        }

        var refreshResult = await _authService.RefreshSignInAsync(request.UserId, cancellationToken);
        if (refreshResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", refreshResult.Error);
            return Result.Failure(refreshResult.Error);
        }

        _logger.LogInformation("Confirmed email change for User {id} successfully", request.UserId);
        return Result.Success();
    }
}
