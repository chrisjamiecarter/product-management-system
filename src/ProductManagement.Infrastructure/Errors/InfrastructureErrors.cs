using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Errors;

internal static class InfrastructureErrors
{
    internal static class User
    {
        internal static readonly Error InvalidSignInAttempt = new(
            "User.InvalidSignInAttempt",
            "Invalid sign in attempt.");

        internal static readonly Error LockedOut = new(
            "User.LockedOut",
            "The user is locked out.");

        internal static readonly Error NotAllowed = new(
            "User.NotAllowed",
            "The user is not allowed to sign in.");

        internal static readonly Error NotRegistered = new(
            "User.NotRegistered",
            "The user was not registered.");

        internal static readonly Error RequiresTwoFactor = new(
            "User.RequiresTwoFactor",
            "The user requires two factor authentication.");
    }
}
