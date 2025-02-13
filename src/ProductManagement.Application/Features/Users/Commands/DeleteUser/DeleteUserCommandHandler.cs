using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Application;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;
using static ProductManagement.Application.Errors.ApplicationErrors;

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
            _logger.LogWarning("{@Error}", userResult.Error);
            return Result.Failure(userResult.Error);
        }

        var validRequestResult = IsValidRequest(request.UserId);
        if (validRequestResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", validRequestResult.Error);
            return Result.Failure(validRequestResult.Error);
        }

        var deleteResult = await _userService.DeleteAsync(request.UserId, cancellationToken);
        if (deleteResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", deleteResult.Error);
            return Result.Failure(deleteResult.Error);
        }

        _logger.LogInformation("Deleted User {id} successfully", request.UserId);
        return Result.Success();
    }

    private Result IsValidRequest(string userId)
    {
        return _currentUserService.UserId != userId
            ? Result.Success()
            : Result.Failure(User.CannotDeleteSelf(userId));
    }
}
