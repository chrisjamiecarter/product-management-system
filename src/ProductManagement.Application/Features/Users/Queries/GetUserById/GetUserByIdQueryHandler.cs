using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Features.Users.Queries.GetUserByEmail;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Users.Queries.GetUserById;

/// <summary>
/// Handles the <see cref="GetUserByIdQuery"/>, and returns a <see cref="GetUserByIdQueryResponse"/>.
/// </summary>
internal sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, GetUserByIdQueryResponse>
{
    private readonly ILogger<GetUserByIdQueryHandler> _logger;
    private readonly IUserService _userService;

    public GetUserByIdQueryHandler(ILogger<GetUserByIdQueryHandler> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public async Task<Result<GetUserByIdQueryResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _userService.FindByIdAsync(request.UserId, cancellationToken);
        if (result.IsFailure)
        {
            _logger.LogWarning("{@Error}", result.Error);
            return Result.Failure<GetUserByIdQueryResponse>(result.Error);
        }

        var response = result.Value.ToResponse();
        _logger.LogInformation("Returned User {id} successfully", request.UserId);
        return Result.Success(response);
    }
}
