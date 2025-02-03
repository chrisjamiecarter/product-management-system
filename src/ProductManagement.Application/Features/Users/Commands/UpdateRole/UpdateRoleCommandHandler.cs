using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Interfaces.Application;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Users.Commands.UpdateRole;

internal sealed class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserService _userService;

    public UpdateRoleCommandHandler(ICurrentUserService currentUserService, IUserService userService)
    {
        _currentUserService = currentUserService;
        _userService = userService;
    }

    public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
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
            return Result.Failure(ApplicationErrors.User.CannotUpdateSelf);
        }

        // Future Enhancements:
        // - There must always be an Owner.
        // - Owners can add/update/delete Owners, Admins, Users.
        // - Admins can only add/update/delete Users.

        var updateResult = await _userService.UpdateRoleAsync(request.UserId, request.Role, cancellationToken);
        if (updateResult.IsFailure)
        {
            return Result.Failure(updateResult.Error);
        }

        return Result.Success();
    }
}
