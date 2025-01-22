using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(string Id,
                                       string Username,
                                       bool EmailConfirmed) : ICommand;
