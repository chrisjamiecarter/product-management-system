using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Users.Commands.CreateUser;

/// <summary>
/// Represents a command to create a new user.
/// </summary>
/// <remarks>
/// This differs from the RegisterCommand.
/// CreateUser is for an authorized user creating an account for another user.
/// Register is for an unauthorized user creating an account for themselves.
/// </remarks>
public sealed record CreateUserCommand(string Email,
                                       string? Role) : ICommand;

