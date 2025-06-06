using Eventix.Modules.Events.Domain.Shared;
using MidR.Interfaces;

namespace Eventix.Modules.Events.Application.Abstractions.Messaging
{
    public interface IQuery<TR> : IRequest<Result<TR>>
    {
    }
}