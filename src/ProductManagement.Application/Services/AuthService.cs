using MediatR;
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

    public async Task<Result> RegisterUserAsync(string email, string password, string confirmUrl, string returnUrl, CancellationToken cancellationToken = default)
    {
        var command = new RegisterUserCommand(email, password, confirmUrl, returnUrl);
        return await _sender.Send(command, cancellationToken);
    }

    public async Task<Result> SignInUserAsync(string email, string password, bool remember, CancellationToken cancellationToken = default)
    {
        var command = new SignInUserCommand(email, password, remember);
        return await _sender.Send(command, cancellationToken);
    }
}
