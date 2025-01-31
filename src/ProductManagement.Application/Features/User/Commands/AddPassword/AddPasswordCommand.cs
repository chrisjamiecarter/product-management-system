using System.Security.Claims;
using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.User.Commands.AddPassword;

public sealed record AddPasswordCommand(string UserId,
                                        string Password) : ICommand;
