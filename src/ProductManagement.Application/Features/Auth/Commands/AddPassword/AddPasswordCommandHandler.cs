using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.AddPassword;

/// <summary>
/// Handles the <see cref="AddPasswordCommand"/> by validating the user, adding the password, 
/// and refreshing the authentication session.
/// </summary>
/// /// <remarks>
/// The <see cref="Handle"/> method will return a Success Result if the user is not found to obfuscate from attackers.
/// </remarks>
internal sealed class AddPasswordCommandHandler : ICommandHandler<AddPasswordCommand>
{
    private readonly ILogger<AddPasswordCommandHandler> _logger;
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public AddPasswordCommandHandler(ILogger<AddPasswordCommandHandler> logger, IAuthService authService, IUserService userService)
    {
        _logger = logger;
        _authService = authService;
        _userService = userService;
    }

    public async Task<Result> Handle(AddPasswordCommand request, CancellationToken cancellationToken)
    {
        var userResult = await _userService.FindByIdAsync(request.UserId, cancellationToken);
        if (userResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", userResult.Error);
            return Result.Success();
        }

        var passwordResult = await _userService.AddPasswordAsync(request.UserId, request.Password, cancellationToken);
        if (passwordResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", passwordResult.Error);
            return Result.Failure(passwordResult.Error);
        }

        var refreshResult = await _authService.RefreshSignInAsync(request.UserId, cancellationToken);
        if (refreshResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", refreshResult.Error);
            return Result.Failure(refreshResult.Error);
        }

        _logger.LogInformation("Added password for User {id} successfully", request.UserId);
        return Result.Success();
    }
}
