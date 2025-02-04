﻿using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.Register;

internal sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    private readonly ILinkBuilderService _linkBuilderService;
    private readonly IUserService _userService;

    public RegisterCommandHandler(IAuthService authService, IEmailService emailService, ILinkBuilderService linkBuilderService, IUserService userService)
    {
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
            return Result.Failure(registerResult.Error);
        }

        var userResult = await _userService.FindByEmailAsync(request.Email, cancellationToken);
        if (userResult.IsFailure)
        {
            return Result.Failure(userResult.Error);
        }
        
        var user = userResult.Value;

        var roleResult = await _authService.AddToRoleAsync(user.Id, request.Role, cancellationToken);
        if (roleResult.IsFailure)
        {
            return Result.Failure(roleResult.Error);
        }

        var tokenResult = await _authService.GenerateEmailConfirmationTokenAsync(request.Email, cancellationToken);
        if (tokenResult.IsFailure)
        {
            return Result.Failure(tokenResult.Error);
        }

        if (!string.IsNullOrWhiteSpace(user.Email))
        {
            var emailConfirmationLink = await _linkBuilderService.BuildEmailConfirmationLinkAsync(user.Id, tokenResult.Value, cancellationToken);
            await _emailService.SendEmailConfirmationAsync(user.Email, emailConfirmationLink, cancellationToken);
        }

        return Result.Success();
    }
}
