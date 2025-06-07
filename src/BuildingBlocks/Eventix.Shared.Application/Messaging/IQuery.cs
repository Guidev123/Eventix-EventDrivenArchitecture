using Eventix.Shared.Domain.Responses;
using MidR.Interfaces;

namespace Eventix.Shared.Application.Messaging
{
    public interface IQuery<TR> : IRequest<Result<TR>>
    {
    }
}