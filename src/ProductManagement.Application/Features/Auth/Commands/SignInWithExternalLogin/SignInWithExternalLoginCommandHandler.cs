using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Features.Auth.Commands.SignIn;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.SignInWithExternalLogin;

/// <summary>
/// Handles the <see cref="SignInCommand"/> by validating the credentials and managing login attempts.
/// </summary>
internal sealed class SignInWithExternalLoginCommandHandler : ICommandHandler<SignInWithExternalLoginCommand>
{
    private readonly ILogger<SignInWithExternalLoginCommandHandler> _logger;
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public SignInWithExternalLoginCommandHandler(ILogger<SignInWithExternalLoginCommandHandler> logger, IAuthService authService, IUserService userService)
    {
        _logger = logger;
        _authService = authService;
        _userService = userService;
    }

    public async Task<Result> Handle(SignInWithExternalLoginCommand request, CancellationToken cancellationToken)
    {
        string userId;
        
        var infoResult = await _authService.GetExternalLoginInfo(cancellationToken);
        if (infoResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", infoResult.Error);
            return Result.Failure(infoResult.Error);
        }

        var info = infoResult.Value;

        var userLoginResult = await _userService.FindByLoginAsync(info.Provider, info.ProviderKey, cancellationToken);
        if (userLoginResult.IsSuccess)
        {
            userId = userLoginResult.Value.Id;
        }
        else 
        {
            var userEmailResult = await _userService.FindByEmailAsync(info.Email, cancellationToken);
            if (userEmailResult.IsFailure)
            {
                var createResult = await _userService.CreateAsync(info.Email, true, cancellationToken);
                if (createResult.IsFailure)
                {
                    _logger.LogWarning("{@Error}", createResult.Error);
                    return Result.Failure(createResult.Error);
                }

                userEmailResult = await _userService.FindByEmailAsync(info.Email, cancellationToken);
                if (userEmailResult.IsFailure)
                {
                    _logger.LogWarning("{@Error}", userEmailResult.Error);
                    return Result.Failure(userEmailResult.Error);
                }
            }

            userId = userEmailResult.Value.Id;

            var addLoginResult = await _authService.AddExternalLoginAsync(userId, info.Provider, info.ProviderKey, info.ProviderDisplayName, cancellationToken);
            if (addLoginResult.IsFailure)
            {
                _logger.LogWarning("{@Error}", addLoginResult.Error);
                return Result.Failure(addLoginResult.Error);
            }
        }

        var signInResult = await _authService.ExternalLoginSignInAsync(userId, info.Provider, info.ProviderKey, cancellationToken);
        if (signInResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", signInResult.Error);
            return Result.Failure(signInResult.Error);
        }

        _logger.LogInformation("Signed in User {id} successfully with {provider} provider", userId, info.Provider);
        return Result.Success();
    }
}
