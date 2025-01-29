using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.ForgotPassword;

internal sealed class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand>
{
    private readonly IAuthService _authService;

    public ForgotPasswordCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken = default)
    {
        // TODO: Break out into GeneratePasswordResetTokenAsync and SendPasswordResetEmailAsync.
        return await _authService.ForgotPasswordAsync(request.Email, request.ResetPasswordUrl, cancellationToken);
    }
}
