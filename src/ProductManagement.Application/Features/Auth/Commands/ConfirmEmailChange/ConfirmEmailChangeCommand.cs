using ProductManagement.Application.Abstractions.Messaging;
using ProductManagement.Application.Models;

namespace ProductManagement.Application.Features.Auth.Commands.ConfirmEmailChange;

public sealed record ConfirmEmailChangeCommand(string UserId,
                                               string Email,
                                               AuthToken Token) : ICommand;
