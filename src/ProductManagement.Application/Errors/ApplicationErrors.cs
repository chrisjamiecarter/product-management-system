using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Errors;

public static class ApplicationErrors
{
    public static class PaginatedList
    {
        public static readonly Error InvalidPageNumber = new(
            "PaginatedList.InvalidPageNumber",
            "The paginated list page number is invalid.");

        public static readonly Error InvalidPageSize = new(
            "PaginatedList.InvalidPageSize",
            "The paginated list page size is invalid.");
    }

    public static class Product
    {
        public static readonly Error NotCreated = new(
            "Product.NotCreated",
            "The product was not created.");

        public static readonly Error NotDeleted = new(
            "Product.NotDeleted",
            "The product was not deleted.");

        public static readonly Error NotFound = new(
            "Product.NotFound",
            "The product was not found.");

        public static readonly Error NotUpdated = new(
            "Product.NotUpdated",
            "The product was not updated.");
    }

    public static class User
    {
        public static readonly Error NotCreated = new(
            "User.NotCreated",
            "The product was not created.");

        public static readonly Error NotDeleted = new(
            "User.NotDeleted",
            "The user was not deleted.");

        public static readonly Error NotFound = new(
            "User.NotFound",
            "The user was not found.");

        public static readonly Error NotRegistered = new(
            "User.NotRegistered",
            "The user was not registered.");

        public static readonly Error NotUpdated = new(
            "User.NotUpdated",
            "The user was not updated.");

        public static readonly Error UsernameTaken = new(
            "User.UsernameTaken",
            "The username is already taken.");
    }
}
