using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Repositories;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Users.Commands.CreateUser;

internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // TODO:
        // - Handle Email send.

        var usernameTaken = await _userRepository.ReturnByUsernameAsync(request.Username, cancellationToken);
        if (usernameTaken != null)
        {
            return Result.Failure(ApplicationErrors.User.UsernameTaken);
        }

        var isCreated = await _userRepository.CreateAsync(request.Username, request.Role, cancellationToken);

        return isCreated ? Result.Success() : Result.Failure(ApplicationErrors.User.NotCreated);
    }
}
