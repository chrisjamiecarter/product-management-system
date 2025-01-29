using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.User.Queries.HasPassword;

internal sealed class HasPasswordQueryHandler : IQueryHandler<HasPasswordQuery, HasPasswordQueryResponse>
{
    private readonly IUserService _userService;

    public HasPasswordQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<HasPasswordQueryResponse>> Handle(HasPasswordQuery request, CancellationToken cancellationToken)
    {
        var hasPasswordResult = await _userService.HasPasswordAsync(request.UserId, cancellationToken);
        return hasPasswordResult.IsSuccess
            ? Result.Success(new HasPasswordQueryResponse(hasPasswordResult.Value))
            : Result.Failure<HasPasswordQueryResponse>(hasPasswordResult.Error);
    }
}
