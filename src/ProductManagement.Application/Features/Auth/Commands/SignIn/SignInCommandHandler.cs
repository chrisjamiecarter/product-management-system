using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.SignIn;

/// <summary>
/// Handles the <see cref="SignInCommand"/> by validating the credentials and managing login attempts.
/// </summary>
internal sealed class SignInCommandHandler : ICommandHandler<SignInCommand>
{
    private readonly ILogger<SignInCommandHandler> _logger;
    private readonly IAuthService _authService;

    public SignInCommandHandler(ILogger<SignInCommandHandler> logger, IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    public async Task<Result> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var signInResult = await _authService.PasswordSignInAsync(request.Email, request.Password, request.Remember, cancellationToken);
        if (signInResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", signInResult.Error);
            return Result.Failure(signInResult.Error);
        }

        _logger.LogInformation("Signed in User {email} successfully", request.Email);
        return Result.Success();
    }
}
