using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.GenerateEmailConfirmation;

/// <summary>
/// Handles the <see cref="GenerateEmailConfirmationCommand"> command by validating the user, 
/// generating an email confirmation token, and sending a confirmation email.
/// </summary>
/// /// <remarks>
/// The <see cref="Handle"/> method will return a Success Result if the user is not found to obfuscate from attackers.
/// </remarks>
internal sealed class GenerateEmailConfirmationCommandHandler : ICommandHandler<GenerateEmailConfirmationCommand>
{
    private readonly ILogger<GenerateEmailConfirmationCommandHandler> _logger;
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    private readonly ILinkBuilderService _linkBuilderService;
    private readonly IUserService _userService;

    public GenerateEmailConfirmationCommandHandler(ILogger<GenerateEmailConfirmationCommandHandler> logger, IAuthService authService, IEmailService emailService, ILinkBuilderService linkBuilderService, IUserService userService)
    {
        _logger = logger;
        _authService = authService;
        _emailService = emailService;
        _linkBuilderService = linkBuilderService;
        _userService = userService;
    }

    public async Task<Result> Handle(GenerateEmailConfirmationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Starting {handler} for Email {email}", nameof(GenerateEmailConfirmationCommandHandler), request.Email);

        var userResult = await _userService.FindByEmailAsync(request.Email, cancellationToken);
        if (userResult.IsFailure)
        {
            _logger.LogWarning("Email {email}: {errorCode} - {errorMessage}", request.Email, userResult.Error.Code, userResult.Error.Message);
            return Result.Success();
        }

        var user = userResult.Value;

        var tokenResult = await _authService.GenerateEmailConfirmationTokenAsync(request.Email, cancellationToken);
        if (tokenResult.IsFailure)
        {
            _logger.LogWarning("Email {email}: {errorCode} - {errorMessage}", request.Email, tokenResult.Error.Code, tokenResult.Error.Message);
            return Result.Failure(tokenResult.Error);
        }

        var emailConfirmationLink = await _linkBuilderService.BuildEmailConfirmationLinkAsync(user.Id, tokenResult.Value, cancellationToken);

        var emailResult = await _emailService.SendEmailConfirmationAsync(request.Email, emailConfirmationLink, cancellationToken);
        if (emailResult.IsFailure)
        {
            _logger.LogWarning("Email {email}: {errorCode} - {errorMessage}", request.Email, emailResult.Error.Code, emailResult.Error.Message);
            return Result.Failure(tokenResult.Error);
        }

        _logger.LogInformation("Finished {handler} for Email {email} successfully", nameof(GenerateEmailConfirmationCommandHandler), request.Email);
        return Result.Success();
    }
}
