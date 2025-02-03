using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Application.Models;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Users.Queries.GetUsersPaginated;

internal sealed class GetUsersPaginatedQueryHandler : IQueryHandler<GetUsersPaginatedQuery, PaginatedList<GetUsersPaginatedQueryResponse>>
{
    private readonly IUserService _userService;

    public GetUsersPaginatedQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<PaginatedList<GetUsersPaginatedQueryResponse>>> Handle(GetUsersPaginatedQuery request, CancellationToken cancellationToken)
    {
        var pageResult = await _userService.GetPageAsync(request.SearchUsername,
                                                         request.SearchRole,
                                                         request.SearchEmailConfirmed,
                                                         request.SortColumn,
                                                         request.SortOrder,
                                                         request.PageNumber,
                                                         request.PageSize,
                                                         cancellationToken);
        if (pageResult.IsFailure)
        {
            return Result.Failure<PaginatedList<GetUsersPaginatedQueryResponse>>(pageResult.Error);
        }

        var page = pageResult.Value;

        var response = PaginatedList<GetUsersPaginatedQueryResponse>.Create(page.Items.Select(u => u.ToQueryResponse()),
                                                                            page.TotalCount,
                                                                            page.PageNumber,
                                                                            page.PageSize);

        return Result.Success(response);
    }
}
