namespace Eventix.Modules.ArchitectureTests.Abstractions
{
    public abstract class BaseTest
    {
        protected const string UsersNamespace = "Eventix.Modules.Users";
        protected const string UsersIntegrationEventsNamespace = "Eventix.Modules.Users.IntegrationEvents";

        protected const string EventsNamespace = "Eventix.Modules.Events";
        protected const string EventsIntegrationEventsNamespace = "Eventix.Modules.Events.IntegrationEvents";

        protected const string TicketingNamespace = "Eventix.Modules.Ticketing";
        protected const string TicketingIntegrationEventsNamespace = "Eventix.Modules.Ticketing.IntegrationEvents";

        protected const string AttendanceNamespace = "Eventix.Modules.Attendance";
        protected const string AttendanceIntegrationEventsNamespace = "Eventix.Modules.Attendance.IntegrationEvents";
    }
}