﻿namespace ProductManagement.Application.Features.Products.Queries.GetProducts;

public sealed record GetProductsQueryResponse(IReadOnlyCollection<GetProductsQueryResponseProduct> Products);
