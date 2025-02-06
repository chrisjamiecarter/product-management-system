using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.GenerateEmailConfirmation;

internal sealed class GenerateEmailConfirmationCommandHandler : ICommandHandler<GenerateEmailConfirmationCommand>
{
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    private readonly ILinkBuilderService _linkBuilderService;
    private readonly IUserService _userService;

    public GenerateEmailConfirmationCommandHandler(IAuthService authService, IEmailService emailService, ILinkBuilderService linkBuilderService, IUserService userService)
    {
        _authService = authService;
        _emailService = emailService;
        _linkBuilderService = linkBuilderService;
        _userService = userService;
    }

    public async Task<Result> Handle(GenerateEmailConfirmationCommand request, CancellationToken cancellationToken)
    {
        var userResult = await _userService.FindByEmailAsync(request.Email, cancellationToken);
        if (userResult.IsFailure)
        {
            // Obfuscate that the user does not exists.
            return Result.Success();
        }

        var user = userResult.Value;
        
        var tokenResult = await _authService.GenerateEmailConfirmationTokenAsync(request.Email, cancellationToken);
        if (tokenResult.IsFailure)
        {
            return Result.Failure(tokenResult.Error);
        }

        var emailConfirmationLink = await _linkBuilderService.BuildEmailConfirmationLinkAsync(user.Id, tokenResult.Value, cancellationToken);

        await _emailService.SendEmailConfirmationAsync(request.Email, emailConfirmationLink, cancellationToken);
        
        return Result.Success();
    }
}
