using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.ForgotPassword;

/// <summary>
/// Represents a command to generate a forgot password request for a user.
/// </summary>
public sealed record ForgotPasswordCommand(string Email) : ICommand;
