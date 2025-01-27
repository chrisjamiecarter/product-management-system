using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.ConfirmEmailChange;

internal sealed class ConfirmEmailChangeCommandHandler : ICommandHandler<ConfirmEmailChangeCommand>
{
    private readonly IAuthService _authService;

    public ConfirmEmailChangeCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<Result> Handle(ConfirmEmailChangeCommand request, CancellationToken cancellationToken)
    {
        return await _authService.ConfirmEmailChangeAsync(request.UserId, request.Email, request.Token, cancellationToken);
    }
}
