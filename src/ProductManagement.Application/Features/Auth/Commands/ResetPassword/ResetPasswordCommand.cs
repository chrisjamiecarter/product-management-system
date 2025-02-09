using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Models;

namespace ProductManagement.Application.Features.Auth.Commands.ResetPassword;

/// <summary>
/// Represents a command to reset a users password.
/// </summary>
public sealed record ResetPasswordCommand(string Email,
                                          string Password,
                                          AuthToken Token) : ICommand;
