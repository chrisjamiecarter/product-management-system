using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.SignOut;

/// <summary>
/// Represents a command to sign out a user.
/// </summary>
public sealed record SignOutCommand() : ICommand;
