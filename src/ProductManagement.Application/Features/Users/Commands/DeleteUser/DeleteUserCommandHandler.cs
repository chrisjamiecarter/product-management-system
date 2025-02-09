using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Interfaces.Application;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Users.Commands.DeleteUser;

/// <summary>
/// Handles the <see cref="DeleteUserCommand"/> by deleting an existing user, ensuring the
/// requesting user cannot delete themselves.
/// </summary>
internal class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
{
    private readonly ILogger<DeleteUserCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserService _userService;

    public DeleteUserCommandHandler(ILogger<DeleteUserCommandHandler> logger, ICurrentUserService currentUserService, IUserService userService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
        _userService = userService;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var userResult = await _userService.FindByIdAsync(request.UserId, cancellationToken);
        if (userResult.IsFailure)
        {
            _logger.LogWarning("Failure during {handler}: {@error}", nameof(DeleteUserCommandHandler), userResult.Error);
            return Result.Failure(userResult.Error);
        }

        var currentUserId = _currentUserService.UserId;
        if (currentUserId == request.UserId)
        {
            var error = ApplicationErrors.User.CannotDeleteSelf;
            _logger.LogWarning("Failure during {handler}: {@error}", nameof(DeleteUserCommandHandler), error);
            return Result.Failure(error);
        }

        var deleteResult = await _userService.DeleteAsync(request.UserId, cancellationToken);
        if (deleteResult.IsFailure)
        {
            _logger.LogWarning("Failure during {handler}: {@error}", nameof(DeleteUserCommandHandler), deleteResult.Error);
            return Result.Failure(deleteResult.Error);
        }

        _logger.LogInformation("Deleted User {id} successfully", request.UserId);
        return Result.Success();
    }
}
