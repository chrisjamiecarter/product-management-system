using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Features.Auth.Commands.SignIn;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.SignInUser;

internal sealed class SignInCommandHandler : ICommandHandler<SignInCommand>
{
    private readonly IAuthService _authService;

    public SignInCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        return await _authService.SignInAsync(request.UserId, cancellationToken);
    }
}
