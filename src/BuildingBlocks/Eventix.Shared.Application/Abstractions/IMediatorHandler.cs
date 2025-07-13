using MidR.Interfaces;

namespace Eventix.Shared.Application.Abstractions
{
    public interface IMediatorHandler
    {
        Task<TResponse> DispatchAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}