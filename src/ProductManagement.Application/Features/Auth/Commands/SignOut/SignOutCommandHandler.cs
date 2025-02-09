using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.SignOut;

/// <summary>
/// Handles the <see cref="SignOutCommand"/>.
/// </summary>
internal sealed class SignOutCommandHandler : ICommandHandler<SignOutCommand>
{
    private readonly IAuthService _authService;

    public SignOutCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result> Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        return await _authService.SignOutAsync(cancellationToken);
    }
}
