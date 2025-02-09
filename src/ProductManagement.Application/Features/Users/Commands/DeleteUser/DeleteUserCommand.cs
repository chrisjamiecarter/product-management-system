using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Users.Commands.DeleteUser;

/// <summary>
/// Represents a command to delete an existing user.
/// </summary>
public sealed record DeleteUserCommand(string UserId) : ICommand;
