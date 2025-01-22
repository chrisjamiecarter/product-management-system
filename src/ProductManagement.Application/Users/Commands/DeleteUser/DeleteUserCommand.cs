using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Users.Commands.DeleteUser;

public sealed record DeleteUserCommand(string Id) : ICommand;
