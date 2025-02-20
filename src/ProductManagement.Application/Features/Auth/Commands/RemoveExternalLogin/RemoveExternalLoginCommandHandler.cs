using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.RemoveExternalLogin;

/// <summary>
/// Handles the <see cref="RemoveExternalLoginCommand"/>.
/// </summary>
internal sealed class RemoveExternalLoginCommandHandler : ICommandHandler<RemoveExternalLoginCommand>
{
    private readonly ILogger<RemoveExternalLoginCommandHandler> _logger;
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public RemoveExternalLoginCommandHandler(ILogger<RemoveExternalLoginCommandHandler> logger, IAuthService authService, IUserService userService)
    {
        _logger = logger;
        _authService = authService;
        _userService = userService;
    }

    public async Task<Result> Handle(RemoveExternalLoginCommand request, CancellationToken cancellationToken)
    {
        var userResult = await _userService.FindByIdAsync(request.UserId, cancellationToken);
        if (userResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", userResult.Error);
            return Result.Failure(userResult.Error);
        }

        var removeResult = await _userService.RemoveExternalLoginAsync(request.UserId, request.Provider, request.ProviderKey, cancellationToken);
        if (removeResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", removeResult.Error);
            return Result.Failure(removeResult.Error);
        }

        var signInResult = await _authService.RefreshSignInAsync(request.UserId, cancellationToken);
        if (signInResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", signInResult.Error);
            return Result.Failure(signInResult.Error);
        }

        return Result.Success();
    }
}
