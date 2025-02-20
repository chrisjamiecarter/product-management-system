using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Commands.LinkExternalLogin;

/// <summary>
/// Handles the <see cref="LinkExternalLoginCommand"/>.
/// </summary>
internal sealed class LinkExternalLoginCommandHandler : ICommandHandler<LinkExternalLoginCommand>
{
    private readonly ILogger<LinkExternalLoginCommandHandler> _logger;
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public LinkExternalLoginCommandHandler(ILogger<LinkExternalLoginCommandHandler> logger, IAuthService authService, IUserService userService)
    {
        _logger = logger;
        _authService = authService;
        _userService = userService;
    }

    public async Task<Result> Handle(LinkExternalLoginCommand request, CancellationToken cancellationToken)
    {
        var infoResult = await _authService.GetExternalLoginInfo(cancellationToken);
        if (infoResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", infoResult.Error);
            return Result.Failure(infoResult.Error);
        }

        var info = infoResult.Value;

        var userResult = await _userService.FindByIdAsync(request.UserId, cancellationToken);
        if (userResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", userResult.Error);
            return Result.Failure(userResult.Error);
        }

        var addLoginResult = await _authService.AddExternalLoginAsync(request.UserId, info.Provider, info.ProviderKey, info.ProviderDisplayName, cancellationToken);
        if (addLoginResult.IsFailure)
        {
            _logger.LogWarning("{@Error}", addLoginResult.Error);
            return Result.Failure(addLoginResult.Error);
        }

        return Result.Success();
    }
}
