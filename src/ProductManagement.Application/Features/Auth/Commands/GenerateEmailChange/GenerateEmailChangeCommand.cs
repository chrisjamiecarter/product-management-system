using System.Security.Claims;
using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.GenerateEmailChange;

public sealed record GenerateEmailChangeCommand(ClaimsPrincipal Principal,
                                                string Email,
                                                string ConfirmUrl) : ICommand;
