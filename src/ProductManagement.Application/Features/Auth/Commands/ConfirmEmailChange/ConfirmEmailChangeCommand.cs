using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.ConfirmEmailChange;

public sealed record ConfirmEmailChangeCommand(string UserId,
                                               string Email,
                                               string Token) : ICommand;
