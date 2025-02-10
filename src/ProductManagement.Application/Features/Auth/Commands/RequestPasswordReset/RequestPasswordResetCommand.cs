using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.RequestPasswordReset;

/// <summary>
/// Represents a command to generate a reset password link for a user.
/// </summary>
public sealed record RequestPasswordResetCommand(string Email) : ICommand;
