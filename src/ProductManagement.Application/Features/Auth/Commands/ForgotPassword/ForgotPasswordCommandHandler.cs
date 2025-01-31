using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.ForgotPassword;

internal sealed class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand>
{
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    private readonly ILinkBuilderService _linkBuilderService;
    private readonly IUserService _userService;

    public ForgotPasswordCommandHandler(IAuthService authService, IEmailService emailService, ILinkBuilderService linkBuilderService, IUserService userService)
    {
        _authService = authService;
        _emailService = emailService;
        _linkBuilderService = linkBuilderService;
        _userService = userService;
    }

    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken = default)
    {
        var userResult = await _userService.FindByEmailAsync(request.Email, cancellationToken);
        if (userResult.IsFailure)
        {
            return Result.Success();
        }

        var tokenResult = await _authService.GeneratePasswordResetTokenAsync(request.Email, cancellationToken);
        if (tokenResult.IsFailure)
        {
            return Result.Failure(tokenResult.Error);
        }
        
        var passwordResetLink = await _linkBuilderService.BuildPasswordResetLinkAsync(tokenResult.Value, cancellationToken);

        await _emailService.SendPasswordResetAsync(request.Email, passwordResetLink, cancellationToken);

        return Result.Success();
    }
}
