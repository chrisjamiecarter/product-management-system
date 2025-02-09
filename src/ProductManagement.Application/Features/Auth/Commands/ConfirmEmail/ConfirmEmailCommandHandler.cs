using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.ConfirmEmail;

/// <summary>
/// Handles the confirm email command by verifying the user's email confirmation token 
/// and updating their authentication status.
/// </summary>
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
        _logger.LogDebug("Starting {handler} for UserId {userId}",
            nameof(ConfirmEmailCommandHandler), request.UserId);

        var result = await _authService.ConfirmEmailAsync(request.UserId, request.Token, cancellationToken);
        if (result.IsFailure)
        {
            _logger.LogWarning("Failure during {handler} for UserId {userId}. {errorCode} - {errorMessage}",
                nameof(ConfirmEmailCommandHandler), request.UserId, result.Error.Code, result.Error.Message);

            return Result.Failure(result.Error);
        }

        _logger.LogInformation("Finished {handler} for UserId {userId} successfully",
            nameof(ConfirmEmailCommandHandler), request.UserId);

        return Result.Success();
    }
}
