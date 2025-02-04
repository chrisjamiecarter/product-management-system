using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.GenerateEmailChange;

internal sealed class GenerateEmailChangeCommandHandler : ICommandHandler<GenerateEmailChangeCommand>
{
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    private readonly ILinkBuilderService _linkBuilderService;
    private readonly IUserService _userService;

    public GenerateEmailChangeCommandHandler(IAuthService authService, IEmailService emailService, ILinkBuilderService linkBuilderService, IUserService userService)
    {
        _authService = authService;
        _emailService = emailService;
        _linkBuilderService = linkBuilderService;
        _userService = userService;
    }

    public async Task<Result> Handle(GenerateEmailChangeCommand request, CancellationToken cancellationToken)
    {
        var userResult = await _userService.FindByIdAsync(request.UserId, cancellationToken);
        if (userResult.IsFailure)
        {
            return Result.Success();
        }

        var duplicateEmailResult = await _userService.FindByEmailAsync(request.UpdatedEmail, cancellationToken);
        if (duplicateEmailResult.IsSuccess)
        {
            return Result.Failure(ApplicationErrors.User.EmailTaken);
        }

        var tokenResult = await _authService.GenerateEmailChangeTokenAsync(request.UserId, request.UpdatedEmail, cancellationToken);
        if (tokenResult.IsFailure)
        {
            return Result.Failure(tokenResult.Error);
        }

        var changeEmailConfirmationLink = await _linkBuilderService.BuildChangeEmailConfirmationLinkAsync(request.UserId, request.UpdatedEmail, tokenResult.Value, cancellationToken);

        await _emailService.SendChangeEmailConfirmationAsync(request.UpdatedEmail, changeEmailConfirmationLink, cancellationToken);

        return Result.Success();
    }
}
