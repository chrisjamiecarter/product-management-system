using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Repositories;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Users.Commands.UpdateUser;

internal sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        // TODO:
        // - Can't update self.
        // - Handle Email send.

        var user = await _userRepository.ReturnByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure(ApplicationErrors.User.NotFound);
        }
        
        if (user.Username != request.Username)
        {
            var usernameTaken = await _userRepository.ReturnByUsernameAsync(request.Username, cancellationToken);
            if (usernameTaken != null)
            {
                return Result.Failure(ApplicationErrors.User.UsernameTaken);
            }
        }

        user.Username = request.Username;
        user.Role = request.Role;
        user.EmailConfirmed = request.EmailConfirmed;

        var isUpdated = await _userRepository.UpdateAsync(user, cancellationToken);

        return isUpdated ? Result.Success() : Result.Failure(ApplicationErrors.User.NotUpdated);
    }
}
