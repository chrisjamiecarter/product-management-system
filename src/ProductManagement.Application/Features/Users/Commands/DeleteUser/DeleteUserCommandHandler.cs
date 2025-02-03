using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Interfaces.Application;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Users.Commands.DeleteUser;

internal class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserService _userService;

    public DeleteUserCommandHandler(ICurrentUserService currentUserService, IUserService userService)
    {
        _currentUserService = currentUserService;
        _userService = userService;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var userResult = await _userService.FindByIdAsync(request.UserId, cancellationToken);
        if (userResult.IsFailure)
        {
            return Result.Failure(userResult.Error);
        }

        // Can't update self.
        var currentUserId = _currentUserService.UserId;
        if (currentUserId == request.UserId)
        {
            return Result.Failure(ApplicationErrors.User.CannotDeleteSelf);
        }

        var deleteResult = await _userService.DeleteAsync(request.UserId, cancellationToken);
        if (deleteResult.IsFailure)
        {
            return Result.Failure(deleteResult.Error);
        }

        return Result.Success();
    }
}
