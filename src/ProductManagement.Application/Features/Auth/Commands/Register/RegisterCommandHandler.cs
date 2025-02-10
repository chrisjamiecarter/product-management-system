using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.Register;

/// <summary>
/// Handles the <see cref="RegisterCommand"/> by creating a new user, generating an email 
/// confirmation token, and sending a confirmation email.
/// </summary>
internal sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly ILogger<RegisterCommandHandler> _logger;
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    private readonly ILinkBuilderService _linkBuilderService;
    private readonly IUserService _userService;

    public RegisterCommandHandler(ILogger<RegisterCommandHandler> logger, IAuthService authService, IEmailService emailService, ILinkBuilderService linkBuilderService, IUserService userService)
    {
        _logger = logger;
        _authService = authService;
        _emailService = emailService;
        _linkBuilderService = linkBuilderService;
        _userService = userService;
    }

    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken = default)
    {
        var registerResult = await _authService.RegisterAsync(request.Email, request.Password, cancellationToken);
        if (registerResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", registerResult.Error);
            return Result.Failure(registerResult.Error);
        }

        var userResult = await _userService.FindByEmailAsync(request.Email, cancellationToken);
        if (userResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", userResult.Error);
            return Result.Failure(userResult.Error);
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

        _logger.LogInformation("Registered User {id} successfully", user.Id);
        return Result.Success();
    }
}
