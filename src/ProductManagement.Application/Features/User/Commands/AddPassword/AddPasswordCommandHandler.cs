using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.User.Commands.AddPassword;

internal sealed class AddPasswordCommandHandler : ICommandHandler<AddPasswordCommand>
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public AddPasswordCommandHandler(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    public async Task<Result> Handle(AddPasswordCommand request, CancellationToken cancellationToken)
    {
        var addPasswordResult = await _userService.AddPasswordAsync(request.UserId, request.Password, cancellationToken);
        if (addPasswordResult.IsFailure)
        {
            return Result.Failure(addPasswordResult.Error);
        }

        var refreshSignInResult = await _authService.RefreshSignInAsync(request.UserId, cancellationToken);
        if (refreshSignInResult.IsFailure)
        {
            return Result.Failure(refreshSignInResult.Error);
        }

        return Result.Success();
    }
}
