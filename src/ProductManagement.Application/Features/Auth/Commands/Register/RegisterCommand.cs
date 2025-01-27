using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.Register;

public sealed record RegisterCommand(string Email,
                                     string Password,
                                     string ConfirmUrl,
                                     string ReturnUrl) : ICommand;
