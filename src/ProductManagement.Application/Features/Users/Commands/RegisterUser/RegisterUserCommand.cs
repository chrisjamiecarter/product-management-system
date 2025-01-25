using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(string Email,
                                         string Password,
                                         string ConfirmEmailUrl,
                                         string ReturnUrl) : ICommand;
