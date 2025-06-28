namespace Eventix.Shared.Presentation.Extensions
{
    public static class PolicyExtensions
    {
        public static readonly string GetUser = "users:read";
        public static readonly string ModifyUser = "users:update";
        public static readonly string GetEvents = "events:read";
        public static readonly string SearchEvents = "events:search";
        public static readonly string ModifyEvents = "events:update";
        public static readonly string GetTicketTypes = "ticket-types:read";
        public static readonly string ModifyTicketTypes = "ticket-types:update";
        public static readonly string GetCategories = "categories:read";
        public static readonly string ModifyCategories = "categories:update";
        public static readonly string GetCart = "carts:read";
        public static readonly string AddToCart = "carts:add";
        public static readonly string RemoveFromCart = "carts:remove";
        public static readonly string GetOrders = "orders:read";
        public static readonly string CreateOrder = "orders:create";
        public static readonly string GetTickets = "tickets:read";
        public static readonly string CheckInTicket = "tickets:check-in";
        public static readonly string GetEventStatistics = "event-statistics:read";
    }
}