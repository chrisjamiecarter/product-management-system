using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Queries.GetExternalLogins;

/// <summary>
/// Handles the <see cref="GetExternalLoginsQuery"/>, and returns a <see cref="GetExternalLoginsQueryResponse"/>.
/// </summary>
internal sealed class GetExternalLoginsQueryHandler : IQueryHandler<GetExternalLoginsQuery, GetExternalLoginsQueryResponse>
{
    private readonly ILogger<GetExternalLoginsQueryHandler> _logger;
    private readonly IAuthService _authService;

    public GetExternalLoginsQueryHandler(ILogger<GetExternalLoginsQueryHandler> logger, IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    public async Task<Result<GetExternalLoginsQueryResponse>> Handle(GetExternalLoginsQuery request, CancellationToken cancellationToken)
    {
        var result = await _authService.GetExternalLogins(request.Id, cancellationToken);
        if (result.IsFailure)
        {
            _logger.LogWarning("{@Error}", result.Error);
            return Result.Failure<GetExternalLoginsQueryResponse>(result.Error);
        }

        var response = result.Value.ToResponse();
        _logger.LogInformation("Returned External Logins for user {id} successfully", request.Id);
        return Result.Success(response);
    }
}
