using MediatR;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Services;

// TODO: Move to Infrastructure layer.
internal class AuthServiceOld
{
    private readonly ISender _sender;

    public AuthServiceOld(ISender sender)
    {
        _sender = sender;
    }

    public async Task<Result> ConfirmEmailAsync(string userId, string token, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
        //var command = new ConfirmEmailCommand(userId, token);
        //return await _sender.Send(command, cancellationToken);
    }

    public async Task<Result> RegisterAsync(string email, string password, string confirmUrl, string returnUrl, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
        //var command = new RegisterUserCommand(email, password, confirmUrl, returnUrl);
        //return await _sender.Send(command, cancellationToken);
    }

    public async Task<Result> ResetPasswordAsync(string email, string password, string token, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
        //var command = new ResetPasswordCommand(email, password, token);
        //return await _sender.Send(command, cancellationToken);
    }

    public async Task<Result> ForgotPasswordAsync(string email, string resetUrl, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
        //var command = new ForgotPasswordCommand(email, resetUrl);
        //return await _sender.Send(command, cancellationToken);
    }

    public async Task<Result> SignInAsync(string email, string password, bool remember, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
        //var command = new SignInUserCommand(email, password, remember);
        //return await _sender.Send(command, cancellationToken);
    }
}
