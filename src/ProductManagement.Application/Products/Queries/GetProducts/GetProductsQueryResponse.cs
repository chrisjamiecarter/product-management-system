namespace ProductManagement.Application.Products.Queries.GetProducts;

public sealed record GetProductsQueryResponse(IReadOnlyCollection<GetProductsQueryResponseProduct> Products);

