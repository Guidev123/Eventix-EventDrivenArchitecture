using Eventix.Modules.Events.Presentation.Events;
using Microsoft.AspNetCore.Routing;

namespace Eventix.Modules.Events.Presentation
{
    public static class EventsEndpoints
    {
        internal const int DEFAULT_PAGE = 1;
        internal const int DEFAULT_PAGE_SIZE = 10;

        public static void MapEndpoints(IEndpointRouteBuilder app)
        {
            CreateEvent.MapEndpoint(app);
            GetEventById.MapEndpoint(app);
            GetAllEvents.MapEndpoint(app);
            CancelEvent.MapEndpoint(app);
            RescheduleEvent.MapEndpoint(app);
            SearchEvent.MapEndpoint(app);
            PublishEvent.MapEndpoint(app);
        }
    }
}