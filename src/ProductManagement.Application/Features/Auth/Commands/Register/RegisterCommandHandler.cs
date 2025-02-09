using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.Register;

/// <summary>
/// Handles the <see cref="RegisterCommand"/> by creating a new user, assigning a role, 
/// generating an email confirmation token, and sending a confirmation email.
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
        _logger.LogDebug("Starting {handler} for Email {email}", nameof(RegisterCommandHandler), request.Email);

        var registerResult = await _authService.RegisterAsync(request.Email, request.Password, cancellationToken);
        if (registerResult.IsFailure)
        {
            _logger.LogWarning("Email {email}: {errorCode} - {errorMessage}", request.Email, registerResult.Error.Code, registerResult.Error.Message);
            return Result.Failure(registerResult.Error);
        }

        var userResult = await _userService.FindByEmailAsync(request.Email, cancellationToken);
        if (userResult.IsFailure)
        {
            _logger.LogWarning("Email {email}: {errorCode} - {errorMessage}", request.Email, userResult.Error.Code, userResult.Error.Message);
            return Result.Failure(userResult.Error);
        }
        
        var user = userResult.Value;

        var roleResult = await _authService.AddToRoleAsync(user.Id, request.Role, cancellationToken);
        if (roleResult.IsFailure)
        {
            _logger.LogWarning("Email {email}: {errorCode} - {errorMessage}", request.Email, roleResult.Error.Code, roleResult.Error.Message);
            return Result.Failure(roleResult.Error);
        }

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

        _logger.LogInformation("Finished {handler} for Email {email} successfully", nameof(RegisterCommandHandler), request.Email);
        return Result.Success();
    }
}
