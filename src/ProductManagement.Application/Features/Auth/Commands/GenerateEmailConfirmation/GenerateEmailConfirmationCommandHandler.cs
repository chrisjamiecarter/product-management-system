using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.GenerateEmailConfirmation;

internal sealed class GenerateEmailConfirmationCommandHandler : ICommandHandler<GenerateEmailConfirmationCommand>
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public GenerateEmailConfirmationCommandHandler(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    public async Task<Result> Handle(GenerateEmailConfirmationCommand request, CancellationToken cancellationToken)
    {
        var userResult = await _userService.FindByEmailAsync(request.Email, cancellationToken);
        if (userResult.IsFailure)
        {
            // Obfuscate that the user does not exists.
            return Result.Success();
        }

        var confirmedResult = await _userService.IsEmailConfirmedAsync(request.Email, cancellationToken);
        if (confirmedResult.IsFailure || confirmedResult.Value)
        {
            // Obfuscate that the email is already confirmed.
            return Result.Success();
        }
        
        var tokenResult = await _authService.GenerateEmailConfirmationTokenAsync(request.Email, cancellationToken);
        if (tokenResult.IsFailure)
        {
            return Result.Failure(tokenResult.Error);
        }

        var emailResult = await _authService.SendEmailConfirmationLinkAsync(request.Email, request.ConfirmUrl, tokenResult.Value, cancellationToken);
        if (emailResult.IsFailure)
        {
            return Result.Failure(emailResult.Error);
        }

        return Result.Success();
    }
}
