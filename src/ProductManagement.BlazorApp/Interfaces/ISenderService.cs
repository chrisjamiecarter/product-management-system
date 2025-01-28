using MediatR;

namespace ProductManagement.BlazorApp.Interfaces;

internal interface ISenderService
{
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}