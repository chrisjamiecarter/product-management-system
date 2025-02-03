using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Users.Queries.GetUserByEmail;

internal sealed class GetUserByEmailQueryHandler : IQueryHandler<GetUserByEmailQuery, GetUserByEmailQueryResponse>
{
    private readonly IUserService _userService;

    public GetUserByEmailQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<GetUserByEmailQueryResponse>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var userResult = await _userService.FindByEmailAsync(request.Email, cancellationToken);
        if (userResult.IsFailure)
        {
            return Result.Failure<GetUserByEmailQueryResponse>(userResult.Error);
        }

        var user = userResult.Value;

        var response = new GetUserByEmailQueryResponse(user.Id, user.Username, user.Role, user.EmailConfirmed);
        return Result.Success(response);
    }
}
