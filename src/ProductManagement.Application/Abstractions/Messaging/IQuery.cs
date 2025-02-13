using MediatR;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Abstractions.Messaging;

/// <summary>
/// Defines a query that returns a <see cref="Result"/> with a response type.
/// </summary>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
