using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Models;
using ProductManagement.Application.Repositories;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Users.Queries.GetUsersPaginated;

internal sealed class GetUsersPaginatedQueryHandler : IQueryHandler<GetUsersPaginatedQuery, PaginatedList<GetUsersPaginatedQueryResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersPaginatedQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<PaginatedList<GetUsersPaginatedQueryResponse>>> Handle(GetUsersPaginatedQuery request, CancellationToken cancellationToken)
    {
        if (request.PageNumber <= 0)
        {
            return Result.Failure<PaginatedList<GetUsersPaginatedQueryResponse>>(ApplicationErrors.PaginatedList.InvalidPageNumber);
        }

        if (request.PageSize <= 0)
        {
            return Result.Failure<PaginatedList<GetUsersPaginatedQueryResponse>>(ApplicationErrors.PaginatedList.InvalidPageSize);
        }

        var users = await _userRepository.ReturnByPageAsync(request.SearchUserName,
                                                            request.SearchEmailConfirmed,
                                                            request.SortOrder,
                                                            request.PageNumber,
                                                            request.PageSize,
                                                            cancellationToken);

        var response = PaginatedList<GetUsersPaginatedQueryResponse>.Create(users.Items.Select(u => u.ToQueryResponse()),
                                                                            users.TotalCount,
                                                                            users.PageNumber,
                                                                            users.PageSize);

        return Result.Success(response);
    }
}
