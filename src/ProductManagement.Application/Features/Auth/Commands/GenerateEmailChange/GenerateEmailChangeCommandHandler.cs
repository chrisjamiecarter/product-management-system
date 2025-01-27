using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.GenerateEmailChange;

internal sealed class GenerateEmailChangeCommandHandler : ICommandHandler<GenerateEmailChangeCommand>
{
    private readonly IAuthService _authService;

    public GenerateEmailChangeCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result> Handle(GenerateEmailChangeCommand request, CancellationToken cancellationToken)
    {
        return await _authService.GenerateEmailChangeAsync(request.Principal, request.Email, request.ConfirmUrl, cancellationToken);
    }
}
