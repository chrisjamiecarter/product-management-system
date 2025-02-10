using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Features.Products.Queries.GetProductsPaginated;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Application.Models;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Users.Queries.GetUsersPaginated;

/// <summary>
/// Handles the <see cref="GetUsersPaginatedQuery"/> by retrieving a page of users that satisfy 
/// the query parameters, and returns a paginated list of <see cref="GetUsersPaginatedQueryResponse"/>.
/// </summary>
internal sealed class GetUsersPaginatedQueryHandler : IQueryHandler<GetUsersPaginatedQuery, PaginatedList<GetUsersPaginatedQueryResponse>>
{
    private readonly ILogger<GetUsersPaginatedQueryHandler> _logger;
    private readonly IUserService _userService;

    public GetUsersPaginatedQueryHandler(ILogger<GetUsersPaginatedQueryHandler> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public async Task<Result<PaginatedList<GetUsersPaginatedQueryResponse>>> Handle(GetUsersPaginatedQuery request, CancellationToken cancellationToken)
    {
        var result = await _userService.GetPageAsync(request.SearchEmail,
                                                         request.SearchEmailConfirmed,
                                                         request.SearchRole,
                                                         request.SortColumn,
                                                         request.SortOrder,
                                                         request.PageNumber,
                                                         request.PageSize,
                                                         cancellationToken);
        if (result.IsFailure)
        {
            _logger.LogWarning("{@Error}", result.Error);
            return Result.Failure<PaginatedList<GetUsersPaginatedQueryResponse>>(result.Error);
        }

        var response = result.Value.ToResponse();
        _logger.LogInformation("Returned Users page {pageNumber} of {totalPages} successfully", response.PageNumber, response.TotalPages);
        return Result.Success(response);
    }
}
