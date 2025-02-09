using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.AddPassword;

/// <summary>
/// Represents a command to add a users password.
/// </summary>
public sealed record AddPasswordCommand(string UserId,
                                        string Password) : ICommand;
