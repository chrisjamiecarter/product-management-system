using MediatR;
using ProductManagement.Domain.Shared;

namespace ProductManagement.Application.Abstractions.Messaging;

/// <summary>
/// Defines a handler for a query that returns a <see cref="Result"/> with a response type.
/// </summary>
/// <typeparam name="TQuery">The type of the query.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>
{
}
