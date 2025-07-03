using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Events.UseCases.GetAll
{
    public sealed record GetAllEventsQuery(int Page, int PageSize) : IQuery<GetAllEventsResponse>
    {
    }
}