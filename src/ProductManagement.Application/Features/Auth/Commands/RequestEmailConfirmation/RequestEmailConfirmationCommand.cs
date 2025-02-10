using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.RequestEmailConfirmation;

/// <summary>
/// Represents a command to generate an email confirmation request for a user.
/// </summary>
public sealed record RequestEmailConfirmationCommand(string Email) : ICommand;
