using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Errors;

internal static class ApplicationErrors
{
    internal static class PaginatedList
    {
        internal static readonly Error InvalidPageNumber = new(
            "PaginatedList.InvalidPageNumber",
            "The paginated list page number is invalid.");

        internal static readonly Error InvalidPageSize = new(
            "PaginatedList.InvalidPageSize",
            "The paginated list page size is invalid.");
    }

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

    internal static class User
    {
        internal static readonly Error NotCreated = new(
            "User.NotCreated",
            "The product was not created.");

        internal static readonly Error NotDeleted = new(
            "User.NotDeleted",
            "The user was not deleted.");

        internal static readonly Error NotFound = new(
            "User.NotFound",
            "The user was not found.");

        internal static readonly Error NotRegistered = new(
            "User.NotRegistered",
            "The user was not registered.");

        internal static readonly Error NotUpdated = new(
            "User.NotUpdated",
            "The user was not updated.");

        internal static readonly Error UsernameTaken = new(
            "User.UsernameTaken",
            "The username is already taken.");
    }
}
