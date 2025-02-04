using ProductManagement.Application.Abstractions.Messaging;

namespace ProductManagement.Application.Features.Auth.Commands.GenerateEmailChange;

// TODO: Replace all instances of OldSomething with CurrentSomething and NewSomething with UpdatedSomething.
public sealed record GenerateEmailChangeCommand(string UserId,
                                                string UpdatedEmail) : ICommand;
