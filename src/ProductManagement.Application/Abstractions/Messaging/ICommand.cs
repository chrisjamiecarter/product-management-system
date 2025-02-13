using MediatR;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Abstractions.Messaging;

/// <summary>
/// Defines a command that returns a <see cref="Result"/>.
/// </summary>
public interface ICommand : IRequest<Result>
{
}

/// <summary>
/// Defines a command that returns a <see cref="Result"/> with a response type.
/// </summary>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
