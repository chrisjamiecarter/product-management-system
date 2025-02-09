﻿using Microsoft.Extensions.Logging;
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
        _logger.LogDebug("Starting {handler} for UserId {userId}", nameof(ConfirmEmailChangeCommandHandler), request.UserId);

        var userResult = await _userService.FindByIdAsync(request.UserId, cancellationToken);
        if (userResult.IsFailure)
        {
            _logger.LogWarning("UserId {userId}: {errorCode} - {errorMessage}", request.UserId, userResult.Error.Code, userResult.Error.Message);
            return Result.Success();
        }

        var emailResult = await _userService.ChangeEmailAsync(request.UserId, request.Email, request.Token, cancellationToken);
        if (emailResult.IsFailure)
        {
            _logger.LogWarning("UserId {userId}: {errorCode} - {errorMessage}", request.UserId, emailResult.Error.Code, emailResult.Error.Message);
            return Result.Failure(emailResult.Error);
        }

        var refreshResult = await _authService.RefreshSignInAsync(request.UserId, cancellationToken);
        if (refreshResult.IsFailure)
        {
            _logger.LogWarning("UserId {userId}: {errorCode} - {errorMessage}", request.UserId, refreshResult.Error.Code, refreshResult.Error.Message);
            return Result.Failure(refreshResult.Error);
        }

        _logger.LogInformation("Finished {handler} for UserId {userId} successfully", nameof(ConfirmEmailChangeCommandHandler), request.UserId);
        return Result.Success();
    }
}
