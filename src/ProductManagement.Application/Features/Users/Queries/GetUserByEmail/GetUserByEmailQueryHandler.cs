using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Features.Users.Queries.GetUserByEmail;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Infrastructure.Handlers;

internal class GetUserByEmailQueryHandler : IQueryHandler<GetUserByEmailQuery, GetUserByEmailQueryResponse>
{
    private readonly IUserRepository _userRepository;

    public GetUserByEmailQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<GetUserByEmailQueryResponse>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.ReturnByEmailAsync(request.Email, cancellationToken);
        if (user is null)
        {
            return Result.Failure<GetUserByEmailQueryResponse>(ApplicationErrors.User.NotFound);
        }

        var response = new GetUserByEmailQueryResponse(user.Id, user.Username, user.Role, user.EmailConfirmed);
        return Result.Success(response);
    }
}
