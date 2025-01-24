using MediatR;
using ProductManagement.Application.Users.Commands.RegisterUser;
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
}
