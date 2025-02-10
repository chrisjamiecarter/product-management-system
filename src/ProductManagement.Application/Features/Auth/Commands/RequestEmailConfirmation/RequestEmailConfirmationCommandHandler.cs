using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.RequestEmailConfirmation;

/// <summary>
/// Handles the <see cref="RequestEmailConfirmationCommand"> command by validating the user, 
/// generating an email confirmation token, and sending a confirmation email.
/// </summary>
/// /// <remarks>
/// The <see cref="Handle"/> method will return a Success Result if the user is not found to obfuscate from attackers.
/// </remarks>
internal sealed class RequestEmailConfirmationCommandHandler : ICommandHandler<RequestEmailConfirmationCommand>
{
    private readonly ILogger<RequestEmailConfirmationCommandHandler> _logger;
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    private readonly ILinkBuilderService _linkBuilderService;
    private readonly IUserService _userService;

    public RequestEmailConfirmationCommandHandler(ILogger<RequestEmailConfirmationCommandHandler> logger, IAuthService authService, IEmailService emailService, ILinkBuilderService linkBuilderService, IUserService userService)
    {
        _logger = logger;
        _authService = authService;
        _emailService = emailService;
        _linkBuilderService = linkBuilderService;
        _userService = userService;
    }

    public async Task<Result> Handle(RequestEmailConfirmationCommand request, CancellationToken cancellationToken)
    {
        var userResult = await _userService.FindByEmailAsync(request.Email, cancellationToken);
        if (userResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", userResult.Error);
            return Result.Success();
        }

        var user = userResult.Value;

        var tokenResult = await _authService.GenerateEmailConfirmationTokenAsync(request.Email, cancellationToken);
        if (tokenResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", tokenResult.Error);
            return Result.Failure(tokenResult.Error);
        }

        var emailConfirmationLink = await _linkBuilderService.BuildEmailConfirmationLinkAsync(user.Id, tokenResult.Value, cancellationToken);

        var emailResult = await _emailService.SendEmailConfirmationAsync(request.Email, emailConfirmationLink, cancellationToken);
        if (emailResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", emailResult.Error);
            return Result.Failure(emailResult.Error);
        }

        _logger.LogInformation("Sent confirm email link for User {id} successfully", user.Id);
        return Result.Success();
    }
}
