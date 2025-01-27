using MediatR;

namespace ProductManagement.BlazorApp.Abstractions.Messaging;

internal sealed class SenderService(ISender sender)
{
    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return await sender.Send(request, cancellationToken);
    }
}
