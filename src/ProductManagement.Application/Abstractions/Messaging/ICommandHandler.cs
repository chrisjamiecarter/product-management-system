using MediatR;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Abstractions.Messaging;

/// <summary>
/// Defines a handler for a command that returns a <see cref="Result"/>.
/// </summary>
/// <typeparam name="TCommand">The type of command.</typeparam>
public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result> where TCommand : ICommand
{
}

/// <summary>
/// Defines a handler for a command that returns a <see cref="Result"/> with a response type.
/// </summary>
/// <typeparam name="TCommand">The type of command.</typeparam>
/// <typeparam name="TResponse">The type of response.</typeparam>
public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse>
{
}
