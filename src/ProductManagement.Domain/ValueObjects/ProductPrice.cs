﻿using ProductManagement.Domain.Errors;
using ProductManagement.Domain.Primitives;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Domain.ValueObjects;

public sealed class ProductPrice : ValueObject
{
    private ProductPrice(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; }

    public static Result<ProductPrice> Create(decimal productPrice)
    {
        if (productPrice < 0)
        {
            return Result.Failure<ProductPrice>(DomainErrors.ProductPrice.NegativeValue);
        }

        return new ProductPrice(productPrice);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
