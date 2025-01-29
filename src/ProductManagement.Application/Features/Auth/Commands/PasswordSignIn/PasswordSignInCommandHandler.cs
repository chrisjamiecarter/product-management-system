using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.PasswordSignIn;

internal sealed class PasswordSignInCommandHandler : ICommandHandler<PasswordSignInCommand>
{
    private readonly IAuthService _authService;

    public PasswordSignInCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result> Handle(PasswordSignInCommand request, CancellationToken cancellationToken)
    {
        return await _authService.PasswordSignInAsync(request.Email, request.Password, request.Remember, cancellationToken);
    }
}
