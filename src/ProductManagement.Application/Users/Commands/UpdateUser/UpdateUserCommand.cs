using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(string UserId,
                                       string Username,
                                       string Role,
                                       bool EmailConfirmed) : ICommand;
