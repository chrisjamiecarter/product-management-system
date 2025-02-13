using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Application;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;
using static ProductManagement.Application.Errors.ApplicationErrors;

namespace ProductManagement.Application.Features.Users.Commands.UpdateRole;

/// <summary>
/// Handles the <see cref="UpdateRoleCommand"/> by updating an existing users role, ensuring the
/// requesting user cannot update their own role.
/// </summary>
internal sealed class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand>
{
    private readonly ILogger<UpdateRoleCommand> _logger;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserService _userService;

    public UpdateRoleCommandHandler(ILogger<UpdateRoleCommand> logger, ICurrentUserService currentUserService, IUserService userService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
        _userService = userService;
    }

    public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var userResult = await _userService.FindByIdAsync(request.UserId, cancellationToken);
        if (userResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", userResult.Error);
            return Result.Failure(userResult.Error);
        }

        var validRequestResult = IsValidRequest(request.UserId);
        if (validRequestResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", validRequestResult.Error);
            return Result.Failure(validRequestResult.Error);
        }

        // Future Enhancements:
        // - There must always be an Owner.
        // - Owners can add/update/delete Owners, Admins, Users.
        // - Admins can only add/update/delete Users.

        var updateResult = await _userService.UpdateRoleAsync(request.UserId, request.Role, cancellationToken);
        if (updateResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", updateResult.Error);
            return Result.Failure(updateResult.Error);
        }

        _logger.LogInformation("Updated User {id} successfully", request.UserId);
        return Result.Success();
    }

    private Result IsValidRequest(string userId)
    {
        return _currentUserService.UserId != userId
            ? Result.Success()
            : Result.Failure(User.CannotUpdateSelf(userId));
    }
}
