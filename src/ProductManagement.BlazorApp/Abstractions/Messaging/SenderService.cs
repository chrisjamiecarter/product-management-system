using MediatR;
using ProductManagement.BlazorApp.Interfaces;

namespace ProductManagement.BlazorApp.Abstractions.Messaging;

/// <summary>
/// Provides a service to send requests using MediatR's ISender.
/// </summary>
/// <param name="sender">The MediatR ISender object.</param>
internal sealed class SenderService(ISender sender) : ISenderService
{
    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return await sender.Send(request, cancellationToken);
    }
}
