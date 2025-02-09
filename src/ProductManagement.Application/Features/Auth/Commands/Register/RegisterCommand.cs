using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.Register;

/// <summary>
/// Represents a command to register a new user.
/// </summary>
/// <remarks>
/// This differs from the CreateUserCommand.
/// CreateUser is for an authorized user creating an account for another user.
/// Register is for an unauthorized user creating an account for themselves.
/// </remarks>
public sealed record RegisterCommand(string Email,
                                     string Password) : ICommand;
