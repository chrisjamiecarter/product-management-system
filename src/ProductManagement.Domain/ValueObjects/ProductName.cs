﻿using ProductManagement.Domain.Errors;
using ProductManagement.Domain.Primitives;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Domain.ValueObjects;

public sealed class ProductName : ValueObject
{
    private ProductName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<ProductName> Create(string productName)
    {
        if (string.IsNullOrWhiteSpace(productName))
        {
            return Result.Failure<ProductName>(DomainErrors.ProductName.Empty);
        }

        return new ProductName(productName);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
