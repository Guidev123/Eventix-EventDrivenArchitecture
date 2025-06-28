using Eventix.ArchitectureTests.Abstractions;
using Eventix.Modules.Ticketing.Domain.Orders.Entities;
using Eventix.Modules.Ticketing.Infrastructure;
using Eventix.Modules.Users.Domain.Users.Entities;
using Eventix.Modules.Users.Infrastructure;
using NetArchTest.Rules;
using System.Reflection;
using Eventix.Modules.Events.Domain.Events.Entities;
using Eventix.Modules.Events.Infrastructure;
using Eventix.Modules.Attendance.Infrastructure;
using Eventix.Modules.Attendance.Domain.Attendees.Entities;
using EventsPresentation = Eventix.Modules.Events.Presentation.PresentationModule;
using AttendancePresentation = Eventix.Modules.Attendance.Presentation.PresentationModule;
using UsersPresentation = Eventix.Modules.Users.Presentation.PresentationModule;
using TicketingPresentation = Eventix.Modules.Ticketing.Presentation.PresentationModule;

namespace Eventix.ArchitectureTests.Layers
{
    public sealed class ModuleTests : BaseTest
    {
        [Fact(DisplayName = "Users Module Should Not Have Dependency On Any Other Module")]
        [Trait("Architecture Tests", "Module Tests")]
        public void UsersModule_ShouldNotHaveDependencyOn_AnyOtherModule()
        {
            // Arrange
            string[] otherModules = [EventsNamespace, AttendanceNamespace, TicketingNamespace];
            string[] integrationEventsModules = [
                EventsIntegrationEventsNamespace,
                AttendanceIntegrationEventsNamespace,
                TicketingIntegrationEventsNamespace
                ];

            var userAssemblies = new Assembly[]
            {
                typeof(User).Assembly,
                Modules.Users.Application.AssemblyReference.Assembly,
                typeof(UsersPresentation).Assembly,
                typeof(UsersModule).Assembly
            };

            // Act & Assert
            Types.InAssemblies(userAssemblies)
                .That()
                .DoNotHaveDependencyOnAny(integrationEventsModules)
                .Should()
                .NotHaveDependencyOnAny(otherModules)
                .GetResult()
                .ShouldBeSuccessful();
        }

        [Fact(DisplayName = "Events Module Should Not Have Dependency On Any Other Module")]
        [Trait("Architecture Tests", "Module Tests")]
        public void EventsModule_ShouldNotHaveDependencyOn_AnyOtherModule()
        {
            // Arrange
            string[] otherModules = [TicketingNamespace, AttendanceNamespace, UsersNamespace];
            string[] integrationEventsModules = [
                TicketingIntegrationEventsNamespace,
                AttendanceIntegrationEventsNamespace,
                UsersNamespace
                ];

            var eventsAssemblies = new Assembly[]
            {
                typeof(Event).Assembly,
                Modules.Events.Application.AssemblyReference.Assembly,
                typeof(EventsPresentation).Assembly,
                typeof(EventsModule).Assembly
            };

            // Act & Assert
            Types.InAssemblies(eventsAssemblies)
                .That()
                .DoNotHaveDependencyOnAny(integrationEventsModules)
                .Should()
                .NotHaveDependencyOnAny(otherModules)
                .GetResult()
                .ShouldBeSuccessful();
        }

        [Fact(DisplayName = "Ticketing Module Should Not Have Dependency On Any Other Module")]
        [Trait("Architecture Tests", "Module Tests")]
        public void TicketingModule_ShouldNotHaveDependencyOn_AnyOtherModule()
        {
            // Arrange
            string[] otherModules = [EventsNamespace, AttendanceNamespace, UsersNamespace];
            string[] integrationEventsModules = [
                EventsIntegrationEventsNamespace,
                AttendanceIntegrationEventsNamespace,
                UsersNamespace
                ];

            var ticketingAssemblies = new Assembly[]
            {
                typeof(Order).Assembly,
                Modules.Ticketing.Application.AssemblyReference.Assembly,
                typeof(TicketingPresentation).Assembly,
                typeof(TicketingModule).Assembly
            };

            // Act & Assert
            Types.InAssemblies(ticketingAssemblies)
                .That()
                .DoNotHaveDependencyOnAny(integrationEventsModules)
                .Should()
                .NotHaveDependencyOnAny(otherModules)
                .GetResult()
                .ShouldBeSuccessful();
        }

        [Fact(DisplayName = "Attendance Module Should Not Have Dependency On Any Other Module")]
        [Trait("Architecture Tests", "Module Tests")]
        public void AttendanceModule_ShouldNotHaveDependencyOn_AnyOtherModule()
        {
            // Arrange
            string[] otherModules = [EventsNamespace, TicketingNamespace, UsersNamespace];
            string[] integrationEventsModules = [
                EventsIntegrationEventsNamespace,
                TicketingIntegrationEventsNamespace,
                UsersNamespace
                ];

            var attendanceAssemblies = new Assembly[]
            {
                typeof(Attendee).Assembly,
                Modules.Attendance.Application.AssemblyReference.Assembly,
                typeof(AttendancePresentation).Assembly,
                typeof(AttendanceModule).Assembly
            };

            // Act & Assert
            Types.InAssemblies(attendanceAssemblies)
                .That()
                .DoNotHaveDependencyOnAny(integrationEventsModules)
                .Should()
                .NotHaveDependencyOnAny(otherModules)
                .GetResult()
                .ShouldBeSuccessful();
        }
    }
}