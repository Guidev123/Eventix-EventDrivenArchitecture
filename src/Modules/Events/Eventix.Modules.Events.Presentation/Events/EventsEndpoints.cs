using Microsoft.AspNetCore.Routing;

namespace Eventix.Modules.Events.Presentation.Events
{
    public static class EventsEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder app)
        {
            CreateEvent.MapEndpoint(app);
            GetEvent.MapEndpoint(app);
        }
    }
}