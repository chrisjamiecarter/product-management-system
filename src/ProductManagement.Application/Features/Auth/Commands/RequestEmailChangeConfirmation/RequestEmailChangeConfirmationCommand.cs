using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.RequestEmailChangeConfirmation;

/// <summary>
/// Represents a command to generate an email change request for a user.
/// </summary>
public sealed record RequestEmailChangeConfirmationCommand(string UserId,
                                                           string UpdatedEmail) : ICommand;
