using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Users.Queries.GetUserByEmail;

/// <summary>
/// Handles the <see cref="GetUserByEmailQuery"/>, and returns a <see cref="GetUserByEmailQueryResponse"/>.
/// </summary>
internal sealed class GetUserByEmailQueryHandler : IQueryHandler<GetUserByEmailQuery, GetUserByEmailQueryResponse>
{
    private readonly ILogger<GetUserByEmailQueryHandler> _logger;
    private readonly IUserService _userService;

    public GetUserByEmailQueryHandler(ILogger<GetUserByEmailQueryHandler> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public async Task<Result<GetUserByEmailQueryResponse>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var result = await _userService.FindByEmailAsync(request.Email, cancellationToken);
        if (result.IsFailure)
        {
            _logger.LogWarning("{@Error}", result.Error);
            return Result.Failure<GetUserByEmailQueryResponse>(result.Error);
        }

        var response = result.Value.ToResponse();
        _logger.LogInformation("Returned User {email} successfully", request.Email);
        return Result.Success(response);
    }
}
