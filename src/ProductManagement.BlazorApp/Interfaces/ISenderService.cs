using MediatR;

namespace ProductManagement.BlazorApp.Interfaces;

/// <summary>
/// Defines the service for sending requests using MediatR's ISender.
/// </summary>
internal interface ISenderService
{
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}