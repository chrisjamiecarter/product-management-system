using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Users.Queries.HasPassword;

/// <summary>
/// Handles the <see cref="HasPasswordQuery"/>, and returns a <see cref="HasPasswordQueryResponse"/>.
/// </summary>
internal sealed class HasPasswordQueryHandler : IQueryHandler<HasPasswordQuery, HasPasswordQueryResponse>
{
    private readonly ILogger<HasPasswordQueryHandler> _logger;
    private readonly IUserService _userService;

    public HasPasswordQueryHandler(ILogger<HasPasswordQueryHandler> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public async Task<Result<HasPasswordQueryResponse>> Handle(HasPasswordQuery request, CancellationToken cancellationToken)
    {
        var result = await _userService.HasPasswordAsync(request.UserId, cancellationToken);
        if (result.IsFailure)
        {
            _logger.LogWarning("{@Error}", result.Error);
            return Result.Failure<HasPasswordQueryResponse>(result.Error);
        }

        var response = result.Value.ToResponse();
        _logger.LogInformation("Returned HasPassword for User {id} successfully", request.UserId);
        return Result.Success(response);
    }
}
