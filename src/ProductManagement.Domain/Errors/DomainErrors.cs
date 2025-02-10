using ProductManagement.Domain.Shared;

namespace ProductManagement.Domain.Errors;

/// <summary>
/// Defines error codes and messages related to the domain.
/// </summary>
public static class DomainErrors
{
    public static class ProductName
    {
        public static readonly Error Empty = new(
            "ProductName.Empty",
            "ProductName is empty.");
    }

    public static class ProductPrice
    {
        public static readonly Error NegativeValue = new(
            "ProductPrice.NegativeValue",
            "ProductPrice is a negative value.");
    }
}
