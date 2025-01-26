using MediatR;
using ProductManagement.Application.Features.Users.Commands.ConfirmEmail;
using ProductManagement.Application.Features.Users.Commands.ForgotPassword;
using ProductManagement.Application.Features.Users.Commands.RegisterUser;
using ProductManagement.Application.Features.Users.Commands.SignInUser;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Services;

internal class AuthService : IAuthService
{
    private readonly ISender _sender;

    public AuthService(ISender sender)
    {
        _sender = sender;
    }

    public async Task<Result> ConfirmEmailAsync(string userId, string token, CancellationToken cancellationToken = default)
    {
        var command = new ConfirmEmailCommand(userId, token);
        return await _sender.Send(command, cancellationToken);
    }

    public async Task<Result> RegisterAsync(string email, string password, string confirmUrl, string returnUrl, CancellationToken cancellationToken = default)
    {
        var command = new RegisterUserCommand(email, password, confirmUrl, returnUrl);
        return await _sender.Send(command, cancellationToken);
    }

    public async Task<Result> ForgotPasswordAsync(string email, string resetUrl, CancellationToken cancellationToken = default)
    {
        var command = new ForgotPasswordCommand(email, resetUrl);
        return await _sender.Send(command, cancellationToken);
    }

    public async Task<Result> SignInAsync(string email, string password, bool remember, CancellationToken cancellationToken = default)
    {
        var command = new SignInUserCommand(email, password, remember);
        return await _sender.Send(command, cancellationToken);
    }
}
