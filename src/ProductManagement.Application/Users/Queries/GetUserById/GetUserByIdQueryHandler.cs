using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Repositories;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Users.Queries.GetUserById;

internal sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, GetUserByIdQueryResponse>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<GetUserByIdQueryResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.ReturnByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<GetUserByIdQueryResponse>(ApplicationErrors.User.NotFound);
        }

        var response = new GetUserByIdQueryResponse(user.Id, user.Username, user.Role, user.EmailConfirmed);
        return Result.Success(response);
    }
}
