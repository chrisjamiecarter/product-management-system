using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Models;

namespace ProductManagement.Application.Features.Auth.Commands.ConfirmEmailChange;

/// <summary>
/// Represents a command to confirm a users email change.
/// </summary>
public sealed record ConfirmEmailChangeCommand(string UserId,
                                               string Email,
                                               AuthToken Token) : ICommand;
