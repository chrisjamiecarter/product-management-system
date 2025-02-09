using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Features.Users.Commands.DeleteUser;
using ProductManagement.Application.Interfaces.Application;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

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
            _logger.LogWarning("Failure during {handler}: {@error}", nameof(UpdateRoleCommandHandler), userResult.Error);
            return Result.Failure(userResult.Error);
        }

        var currentUserId = _currentUserService.UserId;
        if (currentUserId == request.UserId)
        {
            var error = ApplicationErrors.User.CannotUpdateSelf;
            _logger.LogWarning("Failure during {handler}: {@error}", nameof(UpdateRoleCommandHandler), error);
            return Result.Failure(error);
        }

        // Future Enhancements:
        // - There must always be an Owner.
        // - Owners can add/update/delete Owners, Admins, Users.
        // - Admins can only add/update/delete Users.

        var updateResult = await _userService.UpdateRoleAsync(request.UserId, request.Role, cancellationToken);
        if (updateResult.IsFailure)
        {
            _logger.LogWarning("Failure during {handler}: {@error}", nameof(UpdateRoleCommandHandler), updateResult.Error);
            return Result.Failure(updateResult.Error);
        }

        _logger.LogInformation("Updated User {id} successfully", request.UserId);
        return Result.Success();
    }
}
