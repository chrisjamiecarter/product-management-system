﻿using ProductManagement.Domain.Shared;

namespace ProductManagement.Infrastructure.Errors;

internal static class UserErrors
{
    internal static readonly Error EmailConfirmedNotChanged = new(
        "User.EmailNotChanged",
        "There was an error when changing the users email confirmed value.");

    internal static readonly Error EmailNotChanged = new(
        "User.EmailNotChanged",
        "There was an error when changing the users email value.");

    internal static readonly Error NotCreated = new(
        "User.NotCreated",
        "The user was not created.");

    internal static readonly Error NotDeleted = new(
        "User.NotDeleted",
        "The user was not deleted.");

    internal static readonly Error NotFound = new(
        "User.NotFound",
        "The user was not found.");

    internal static readonly Error UsernameNotChanged = new(
        "User.UsernameNotChanged",
        "There was an error when changing the users username value.");

    internal static readonly Error PasswordNotAdded = new(
        "User.PasswordNotAdded",
        "Password not added. Passwords must be at least 8 characters long, and contain at least one lowercase, one uppercase, one numeric and one non-alphanumeric character.");

    // TODO: rationalise the below.

    internal static readonly Error EmailNotConfirmed = new(
        "User.EmailNotConfirmed",
        "The user does not have a confirmed email.");

    internal static readonly Error PasswordNotChanged = new(
        "User.PasswordNotChanged",
        "Password not changed. Passwords must be at least 8 characters long, and contain at least one lowercase, one uppercase, one numeric and one non-alphanumeric character.");

    internal static readonly Error InvalidSecurityStamp = new(
        "User.InvalidSecurityStamp",
        "Invalid security stamp.");

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

    internal static readonly Error NotRegistered = new(
        "User.NotRegistered",
        "The user was not registered.");

    internal static readonly Error RequiresTwoFactor = new(
        "User.RequiresTwoFactor",
        "The user requires two factor authentication.");
}
