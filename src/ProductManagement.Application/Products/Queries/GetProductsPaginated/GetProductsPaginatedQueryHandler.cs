using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Errors;
using ProductManagement.Application.Models;
using ProductManagement.Application.Repositories;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Products.Queries.GetProductsPaginated;

internal sealed class GetProductsPaginatedQueryHandler : IQueryHandler<GetProductsPaginatedQuery, PaginatedList<GetProductsPaginatedQueryResponse>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsPaginatedQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<PaginatedList<GetProductsPaginatedQueryResponse>>> Handle(GetProductsPaginatedQuery request, CancellationToken cancellationToken)
    {
        if (request.PageNumber <= 0)
        {
            return Result.Failure<PaginatedList<GetProductsPaginatedQueryResponse>>(ApplicationErrors.PaginatedList.InvalidPageNumber);
        }

        if (request.PageSize <= 0)
        {
            return Result.Failure<PaginatedList<GetProductsPaginatedQueryResponse>>(ApplicationErrors.PaginatedList.InvalidPageSize);
        }

        var products = await _productRepository.ReturnByPageAsync(request.SearchName,
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

        var response = PaginatedList<GetProductsPaginatedQueryResponse>.Create(
            products.Items.Select(p => p.ToQueryResponse()),
            products.TotalCount,
            products.PageNumber,
            products.PageSize);

        return Result.Success(response);
    }
}
