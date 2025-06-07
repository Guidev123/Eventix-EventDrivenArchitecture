using Eventix.Shared.Domain.Responses;
using MidR.Interfaces;

namespace Eventix.Shared.Application.Messaging
{
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>;
}