using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Errors;

/// <summary>
/// Defines error codes and messages related to the application.
/// </summary>
public static class ApplicationErrors
{
    public static class Email
    {
        public static Error NotSent(string toEmailAddress) => new(
            "Email.NotSent",
            $"Unable to send email to {toEmailAddress}");
    }

    public static class ExternalLogin
    {
        public static Error NotAdded(string email) => new(
            "ExternalLogin.NotAdded",
            $"Unable to add external login for {email}");

        public static readonly Error NotFound = new(
            "ExternalLogin.NotFound",
            "Unable to find external login for the current login");

        public static readonly Error NullEmailClaim = new(
            "ExternalLogin.NullEmailClaim",
            "Unable to find an email claim from the external login for the current login");
    }

    public static class PaginatedList
    {
        public static Error InvalidPageNumber(int pageNumber) => new(
            "PaginatedList.InvalidPageNumber",
            $"The page number {pageNumber} is invalid");

        public static Error InvalidPageSize(int pageSize) => new(
            "PaginatedList.InvalidPageSize",
            $"The page size {pageSize} is invalid");
    }

    public static class Product
    {
        public static readonly Error NotCreated = new(
            "Product.NotCreated",
            "Unable to create product");

        public static Error NotDeleted(Guid id) => new(
            "Product.NotDeleted",
            $"Unable to delete product {id}");

        public static Error NotFound(Guid id) => new(
            "Product.NotFound",
            $"Unable to find product {id}");

        public static Error NotUpdated(Guid id) => new(
            "Product.NotUpdated",
            $"Unable to update product {id}");
    }

    public static class Role
    {
        public static Error InvalidRole(string role) => new(
            "Role.InvalidRole",
            $"The role {role} is invalid");
    }

    public static class User
    {
        public static Error CannotDeleteSelf(string id) => new(
            "User.CannotDeleteSelf",
            $"User {id} cannot delete self");

        public static Error CannotUpdateSelf(string id) => new(
            "User.CannotUpdateSelf",
            $"User {id} cannot update self");

        public static Error EmailNotFound(string email) => new(
            "User.EmailNotFound",
            $"Unable to find user with email {email}");

        public static Error EmailTaken(string email) => new(
            "User.EmailTaken",
            $"The email {email} is already linked to another account");

        public static Error InvalidSecurityStamp(string id) => new(
            "User.InvalidSecurityStamp",
            $"Invalid security stamp for {id}");

        public static Error InvalidSignInAttempt(string email) => new(
            "User.InvalidSignInAttempt",
            $"Invalid sign in attempt for {email}");

        public static Error LockedOut(string email) => new(
            "User.LockedOut",
            $"The user {email} is locked out");

        public static readonly Error LoginNotFound = new(
            "User.LoginNotFound",
            $"A login was not found.");

        public static Error NotAllowed(string email) => new(
            "User.NotAllowed",
            $"The user {email} is not allowed to sign in");

        public static Error NotFound(string id) => new(
            "User.IdNotFound",
            $"Unable to find user {id}");

        public static Error RequiresTwoFactor(string email) => new(
            "User.RequiresTwoFactor",
            $"The user {email} requires two factor authentication");
    }
}
