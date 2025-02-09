using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Models;

namespace ProductManagement.Application.Features.Auth.Commands.ConfirmEmail;

/// <summary>
/// Represents a command to confirm a users email.
/// </summary>
public sealed record ConfirmEmailCommand(string UserId,
                                         AuthToken Token) : ICommand;
