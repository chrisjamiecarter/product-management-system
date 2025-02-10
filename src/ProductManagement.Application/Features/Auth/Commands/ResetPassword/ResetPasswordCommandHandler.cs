using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.ResetPassword;

/// <summary>
/// Handles the <see cref="ResetPasswordCommand"/> by validating the token and updating the user's password.
/// </summary>
/// /// <remarks>
/// The <see cref="Handle"/> method will return a Success Result if the user is not found to obfuscate from attackers.
/// </remarks>
internal sealed class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand>
{
    private readonly ILogger<ResetPasswordCommandHandler> _logger;
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public ResetPasswordCommandHandler(ILogger<ResetPasswordCommandHandler> logger, IAuthService authService, IUserService userService)
    {
        _logger = logger;
        _authService = authService;
        _userService = userService;
    }

    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var userResult = await _userService.FindByEmailAsync(request.Email, cancellationToken);
        if (userResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", userResult.Error);
            return Result.Success();
        }

        var user = userResult.Value;

        var resetResult = await _authService.ResetPasswordAsync(request.Email, request.Password, request.Token, cancellationToken);
        if (resetResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", resetResult.Error);
            return Result.Failure(resetResult.Error);
        }

        _logger.LogInformation("Reset password for User {id} successfully", user.Id);
        return Result.Success();
    }
}
