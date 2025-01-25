using Microsoft.AspNetCore.Identity;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Features.Users.Commands.SignInUser;
using ProductManagement.Domain.Shared;
using ProductManagement.Infrastructure.Database.Identity;

namespace ProductManagement.Infrastructure.Handlers;

internal sealed class SignInUserCommandHandler : ICommandHandler<SignInUserCommand>
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public SignInUserCommandHandler(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<Result> Handle(SignInUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, request.Remember, false);

        // TODO: ERRORS.
        if (result.Succeeded)
        {
            return Result.Success();
        }

        if (result.IsLockedOut)
        {
            return Result.Failure(InfrastructureErrors.User.LockedOut);
        }

        if (result.IsNotAllowed)
        {
            return Result.Failure(InfrastructureErrors.User.NotAllowed);
        }

        if (result.RequiresTwoFactor)
        {
            return Result.Failure(InfrastructureErrors.User.RequiresTwoFactor);
        }

        return Result.Failure(InfrastructureErrors.User.InvalidSignInAttempt);
    }
}
