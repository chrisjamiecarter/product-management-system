using Microsoft.AspNetCore.Identity;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Features.Users.Commands.ConfirmEmail;
using ProductManagement.Domain.Shared;
using ProductManagement.Infrastructure.Database.Identity;
using ProductManagement.Infrastructure.Errors;

namespace ProductManagement.Infrastructure.Handlers;

internal sealed class ConfirmEmailCommandHandler : ICommandHandler<ConfirmEmailCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ConfirmEmailCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null)
        {
            return Result.Failure(InfrastructureErrors.User.NotFound);
        }

        var result = await _userManager.ConfirmEmailAsync(user, request.Token);

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(InfrastructureErrors.User.ErrorConfirmingEmail);
    }
}
