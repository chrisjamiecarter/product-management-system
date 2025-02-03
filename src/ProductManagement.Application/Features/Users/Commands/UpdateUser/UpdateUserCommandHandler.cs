using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Users.Commands.UpdateUser;

internal sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
{
    private readonly IUserService _userService;

    public UpdateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        // TODO:
        // Required? Or UpdateRole?
        // - Can't update self.
        // - Handle Email send.

        var userResult = await _userService.FindByIdAsync(request.UserId, cancellationToken);
        if (userResult.IsFailure)
        {
            return Result.Failure(userResult.Error);
        }


        // TODO: Username Taken?


        // TODO: Update?
        //var isUpdated = await _userService.UpdateAsync(user, cancellationToken);
        //return isUpdated ? Result.Success() : Result.Failure(ApplicationErrors.User.NotUpdated);
        
        return Result.Success();
    }
}
