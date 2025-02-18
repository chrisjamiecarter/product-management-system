using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Users.Commands.CreateUser;

/// <summary>
/// Handles the <see cref="CreateUserCommand"/> by creating a new user, assigning a role, 
/// generating an email confirmation token, and sending a confirmation email.
/// </summary>
internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
{
    private readonly ILogger<CreateUserCommandHandler> _logger;
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    private readonly ILinkBuilderService _linkBuilderService;
    private readonly IUserService _userService;

    public CreateUserCommandHandler(ILogger<CreateUserCommandHandler> logger, IAuthService authService, IEmailService emailService, ILinkBuilderService linkBuilderService, IUserService userService)
    {
        _logger = logger;
        _authService = authService;
        _emailService = emailService;
        _linkBuilderService = linkBuilderService;
        _userService = userService;
    }

    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var createResult = await _userService.CreateAsync(request.Email, false, cancellationToken);
        if (createResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", createResult.Error);
            return Result.Failure(createResult.Error);
        }

        var userResult = await _userService.FindByEmailAsync(request.Email, cancellationToken);
        if (userResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", userResult.Error);
            return Result.Failure(userResult.Error);
        }

        var user = userResult.Value;

        var roleResult = await _userService.UpdateRoleAsync(user.Id, request.Role, cancellationToken);
        if (roleResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", roleResult.Error);
            return Result.Failure(roleResult.Error);
        }

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

        _logger.LogInformation("Created User {id} successfully", user.Id);
        return Result.Success();
    }
}
