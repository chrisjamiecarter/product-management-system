using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.SignIn;

/// <summary>
/// Represents a command to sign in a user using email and password.
/// </summary>
public sealed record SignInCommand(string Email,
                                   string Password,
                                   bool Remember) : ICommand;
