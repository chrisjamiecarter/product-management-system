using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Errors;

internal static class ApplicationErrors
{
    internal static class Product
    {
        internal static readonly Error NotCreated = new(
            "Product.NotCreated",
            "The product was not created.");

        internal static readonly Error NotDeleted = new(
            "Product.NotDeleted",
            "The product was not deleted.");

        internal static readonly Error NotFound = new(
            "Product.NotFound",
            "The product was not found.");
        
        internal static readonly Error NotUpdated = new(
            "Product.NotUpdated",
            "The product was not updated.");
    }
}
