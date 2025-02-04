using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.ChangePassword;

internal sealed class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public ChangePasswordCommandHandler(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var passwordResult = await _userService.ChangePasswordAsync(request.UserId, request.CurrentPassword, request.UpdatedPassword, cancellationToken);
        if (passwordResult.IsFailure)
        {
            return Result.Failure(passwordResult.Error);
        }

        var refreshResult = await _authService.RefreshSignInAsync(request.UserId, cancellationToken);
        if (refreshResult.IsFailure)
        {
            return Result.Failure(refreshResult.Error);
        }

        return Result.Success();
    }
}
