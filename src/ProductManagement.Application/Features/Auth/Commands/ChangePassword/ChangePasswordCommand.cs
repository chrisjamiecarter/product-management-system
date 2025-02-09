using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.ChangePassword;

/// <summary>
/// Represents a command to change a users password.
/// </summary>
public sealed record ChangePasswordCommand(string UserId,
                                           string CurrentPassword,
                                           string UpdatedPassword) : ICommand;
