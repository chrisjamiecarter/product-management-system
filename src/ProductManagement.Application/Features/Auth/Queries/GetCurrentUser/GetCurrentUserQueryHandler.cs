using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Auth.Queries.GetCurrentUser;

internal sealed class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserQuery, GetCurrentUserQueryResponse>
{
    private readonly IAuthService _authService;

    public GetCurrentUserQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<GetCurrentUserQueryResponse>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _authService.GetCurrentUserAsync(request.Principal, cancellationToken);
        
        return user.IsSuccess
            ? Result.Success(new GetCurrentUserQueryResponse(user.Value.Id, user.Value.Username, user.Value.Role, user.Value.EmailConfirmed))
            : Result.Failure<GetCurrentUserQueryResponse>(user.Error);
    }
}
