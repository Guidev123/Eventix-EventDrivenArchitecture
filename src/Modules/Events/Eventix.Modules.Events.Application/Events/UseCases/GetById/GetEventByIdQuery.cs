using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Events.UseCases.GetById
{
    public sealed record GetEventByIdQuery(Guid EventId) : IQuery<GetEventResponse>
    {
    }
}