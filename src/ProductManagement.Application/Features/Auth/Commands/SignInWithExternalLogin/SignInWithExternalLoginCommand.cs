using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.SignInWithExternalLogin;

/// <summary>
/// Represents a command to sign in a user using an external login provider.
/// </summary>
public sealed record SignInWithExternalLoginCommand() : ICommand;
