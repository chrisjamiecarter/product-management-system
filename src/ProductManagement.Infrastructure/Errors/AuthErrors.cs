using ProductManagement.Domain.Shared;

namespace ProductManagement.Infrastructure.Errors;

internal static class AuthErrors
{
    internal static readonly Error EmailNotConfirmed = new(
        "User.EmailNotConfirmed",
        "The user does not have a confirmed email.");

    internal static readonly Error ErrorChangingPassword = new(
        "User.ErrorChangingPassword",
        "There was an error when changing the users password.");
    
    internal static readonly Error ErrorConfirmingEmail = new(
        "User.ErrorConfirmingEmail",
        "There was an error when confirming the users email.");

    internal static readonly Error ErrorConfirmingEmailChange = new(
        "User.ErrorConfirmingEmailChange",
        "There was an error when confirming the change to the users email.");

    internal static readonly Error PasswordNotReset = new(
        "User.PasswordNotReset",
        "There was an error when resetting the users password.");

    internal static readonly Error InvalidSignInAttempt = new(
        "User.InvalidSignInAttempt",
        "Invalid sign in attempt.");

    internal static readonly Error LockedOut = new(
        "User.LockedOut",
        "The user is locked out.");
        
    internal static readonly Error NotAddedToRole = new(
        "User.NotAddedToRole",
        "The user was not added to the role.");

    internal static readonly Error NotAllowed = new(
        "User.NotAllowed",
        "The user is not allowed to sign in.");

    internal static readonly Error NotFound = new(
        "User.NotFound",
        "The user was not found.");

    internal static readonly Error NotRegistered = new(
        "User.NotRegistered",
        "The user was not registered.");

    internal static readonly Error RequiresTwoFactor = new(
        "User.RequiresTwoFactor",
        "The user requires two factor authentication.");
}
