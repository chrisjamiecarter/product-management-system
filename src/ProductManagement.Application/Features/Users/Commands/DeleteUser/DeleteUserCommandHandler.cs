using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Users.Commands.DeleteUser;

internal class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        // TODO:
        // - Can't delete self.

        var user = await _userRepository.ReturnByIdAsync(request.Id, cancellationToken);
        if (user is null)
        {
            return Result.Failure(ApplicationErrors.User.NotFound);
        }

        var isDeleted = await _userRepository.DeleteAsync(user, cancellationToken);

        return isDeleted ? Result.Success() : Result.Failure(ApplicationErrors.User.NotDeleted);
    }
}
