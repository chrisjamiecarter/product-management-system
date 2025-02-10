using Microsoft.Extensions.Logging;
using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Interfaces.Infrastructure;
using ProductManagement.Application.Models;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Features.Products.Queries.GetProductsPaginated;

/// <summary>
/// Handles the <see cref="GetProductsPaginatedQuery"/> by retrieving a page of products that satisfy 
/// the query parameters, and returns a paginated list of <see cref="GetProductsPaginatedQueryResponse"/>.
/// </summary>
internal sealed class GetProductsPaginatedQueryHandler : IQueryHandler<GetProductsPaginatedQuery, PaginatedList<GetProductsPaginatedQueryResponse>>
{
    private readonly ILogger<GetProductsPaginatedQueryHandler> _logger;
    private readonly IProductRepository _productRepository;

    public GetProductsPaginatedQueryHandler(ILogger<GetProductsPaginatedQueryHandler> logger, IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }

    public async Task<Result<PaginatedList<GetProductsPaginatedQueryResponse>>> Handle(GetProductsPaginatedQuery request, CancellationToken cancellationToken)
    {
        var result = await _productRepository.ReturnByPageAsync(request.SearchName,
                                                                request.SearchIsActive,
                                                                request.SearchFromAddedOnDateUtc,
                                                                request.SearchToAddedOnDateUtc,
                                                                request.SearchFromPrice,
                                                                request.SearchToPrice,
                                                                request.SortColumn,
                                                                request.SortOrder,
                                                                request.PageNumber,
                                                                request.PageSize,
                                                                cancellationToken);
        if (result.IsFailure)
        {
            _logger.LogWarning("{@Error}", result.Error);
            return Result.Failure<PaginatedList<GetProductsPaginatedQueryResponse>>(result.Error);
        }

        var response = result.Value.ToResponse();
        _logger.LogInformation("Returned Products page {pageNumber} of {totalPages} successfully", response.PageNumber, response.TotalPages);
        return Result.Success(response);
    }
}
