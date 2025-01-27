using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.ResetPassword;
public sealed record ResetPasswordCommand(string Email,
                                          string Password,
                                          string Token) : ICommand;
