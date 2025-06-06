using Eventix.Modules.Events.Domain.Shared;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Application.Abstractions.Messaging
{
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>;
}