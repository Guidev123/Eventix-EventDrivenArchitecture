using Eventix.Modules.Events.Presentation.Categories;
using Eventix.Modules.Events.Presentation.Events;
using Eventix.Modules.Events.Presentation.TicketTypes;
using Microsoft.AspNetCore.Routing;

namespace Eventix.Modules.Events.Presentation
{
    public static class EventsEndpoints
    {
        internal const int DEFAULT_PAGE = 1;
        internal const int DEFAULT_PAGE_SIZE = 10;

        public static void MapEndpoints(IEndpointRouteBuilder app)
        {
            MapEventEndpoints(app);
            MapTicketTypeEndpoints(app);
            MapCategoryEndpoints(app);
        }

        private static void MapEventEndpoints(this IEndpointRouteBuilder app)
        {
            CreateEvent.MapEndpoint(app);
            GetEventById.MapEndpoint(app);
            GetAllEvents.MapEndpoint(app);
            CancelEvent.MapEndpoint(app);
            RescheduleEvent.MapEndpoint(app);
            SearchEvent.MapEndpoint(app);
            PublishEvent.MapEndpoint(app);
        }

        private static void MapTicketTypeEndpoints(this IEndpointRouteBuilder app)
        {
            CreateTicketType.MapEndpoint(app);
            GetTicketTypeById.MapEndpoint(app);
            GetAllTicketTypes.MapEndpoint(app);
            UpdateTicketTypePrice.MapEndpoint(app);
        }

        private static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
        {
            GetAllCategories.MapEndpoint(app);
            GetCategoryById.MapEndpoint(app);
            ArchiveCategory.MapEndpoint(app);
            CreateCategory.MapEndpoint(app);
            UpdateCategory.MapEndpoint(app);
        }
    }
}