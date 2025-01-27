using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.GenerateEmailConfirmation;

internal sealed class GenerateEmailConfirmationCommandHandler : ICommandHandler<GenerateEmailConfirmationCommand>
{
    private readonly IAuthService _authService;

    public GenerateEmailConfirmationCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result> Handle(GenerateEmailConfirmationCommand request, CancellationToken cancellationToken)
    {
        return await _authService.GenerateEmailConfirmationAsync(request.Email, request.ConfirmUrl, request.ReturnUrl, cancellationToken);
    }
}
