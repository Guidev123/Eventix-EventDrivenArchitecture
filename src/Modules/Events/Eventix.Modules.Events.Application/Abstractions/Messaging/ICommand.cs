using Eventix.Modules.Events.Domain.Shared;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Application.Abstractions.Messaging
{
    public interface ICommand : IRequest<Result>, IBaseCommand;

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;

    public interface IBaseCommand;
}