using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.Register;

internal sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken = default)
    {
        // TODO:
        // Maybe do:
        //     var registerResult = await _authService.RegisterAsync(request.Email, request.Password, cancellationToken);
        //
        //     var userResult = await _userService.GetUserByEmail(request.Email, cancellationToken);
        //     var user = userResult.Value;
        // or
        //     return the user as part of the registerResult.
        //
        //     var tokenResult = await _authService.GenerateEmailConfirmationTokenAsync(user, cancellationToken);
        
        var registerResult = await _authService.RegisterAsync(request.Email, request.Password, cancellationToken);
        if (registerResult.IsFailure)
        {
            return registerResult;
        }

        var roleResult = await _authService.AddToRoleAsync(request.Email, request.Role, cancellationToken);
        if (roleResult.IsFailure)
        {
            return roleResult;
        }

        var tokenResult = await _authService.GenerateEmailConfirmationTokenAsync(request.Email, cancellationToken);
        if (tokenResult.IsFailure)
        {
            return tokenResult;
        }

        var emailResult = await _authService.SendEmailConfirmationLinkAsync(request.Email, request.ConfirmUrl, tokenResult.Value, cancellationToken);
        if (emailResult.IsFailure)
        {
            return emailResult;
        }

        return Result.Success();
    }
}
