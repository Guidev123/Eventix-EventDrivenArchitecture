using Eventix.Shared.Domain.Responses;
using MidR.Interfaces;

namespace Eventix.Shared.Application.Messaging
{
    public interface ICommand : IRequest<Result>, IBaseCommand;

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;

    public interface IBaseCommand;
}