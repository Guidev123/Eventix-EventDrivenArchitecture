using Eventix.Modules.Attendance.Domain.Attendees.Interfaces;
using Eventix.Modules.Attendance.Infrastructure.Attendees.Repositories;
using Eventix.Modules.Attendance.Infrastructure.Database;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Infrastructure.Outbox.Interceptors;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using Eventix.Shared.Presentation.Extensions;
using Eventix.Modules.Attendance.Presentation;
using Eventix.Modules.Attendance.Infrastructure.Inbox;
using Eventix.Modules.Attendance.Infrastructure.Outbox;
using Eventix.Modules.Attendance.Domain.Events.Interfaces;
using Eventix.Modules.Attendance.Infrastructure.Events.Repositories;
using Eventix.Modules.Attendance.Domain.Tickets.Interfaces;
using Eventix.Modules.Attendance.Infrastructure.Tickets.Repositories;

namespace Eventix.Modules.Attendance.Infrastructure
{
    public static class AttendanceModule
    {
        private const string DATABASE_CONNECTION = "Database";
        private const string CONNECTION_ERROR_MESSAGE = $"The connection string {DATABASE_CONNECTION} is not configured";

        public static IServiceCollection AddAttendanceModule(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddEndpoints(typeof(PresentationModule).Assembly)
                .AddRepositories()
                .AddDomainEventHandlers()
                .AddIntegrationEventHandlers()
                .AddOutbox(configuration)
                .AddInbox(configuration)
                .AddEntityFrameworkDbContext(configuration);

            return services;
        }

        public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator)
        {
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAttendeeRepository, AttendeeRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();

            return services;
        }

        private static IServiceCollection AddEntityFrameworkDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(DATABASE_CONNECTION)
                ?? throw new InvalidOperationException(CONNECTION_ERROR_MESSAGE);

            services.AddDbContext<AttendanceDbContext>((sp, options) =>
            {
                var insertOutboxMessagesInterceptor = sp.GetRequiredService<InsertOutboxMessagesInterceptors>();

                options.UseSqlServer(connectionString).AddInterceptors(insertOutboxMessagesInterceptor).LogTo(Console.WriteLine);
            });

            return services;
        }

        private static IServiceCollection AddOutbox(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OutboxOptions>(configuration.GetSection("Attendance:Outbox"));
            services.ConfigureOptions<ConfigureProcessOutboxJob>();

            return services;
        }

        private static IServiceCollection AddInbox(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<InboxOptions>(configuration.GetSection("Attendance:Inbox"));
            services.ConfigureOptions<ConfigureProcessInboxJob>();

            return services;
        }

        private static IServiceCollection AddDomainEventHandlers(this IServiceCollection services)
        {
            var domainEventHandlers = Application.AssemblyReference.Assembly
                .GetTypes()
                .Where(c => c.IsAssignableTo(typeof(IDomainEventHandler)))
                .ToArray();

            foreach (var domainEventHandler in domainEventHandlers)
            {
                services.TryAddTransient(domainEventHandler);

                var domainEvent = domainEventHandler
                    .GetInterfaces()
                    .Single(c => c.IsGenericType)
                    .GetGenericArguments()
                    .Single();

                var closedIdempotentHandler = typeof(AttendanceIdempotentDomainEventHandler<>)
                    .MakeGenericType(domainEvent);

                services.Decorate(domainEventHandler, closedIdempotentHandler);
            }

            return services;
        }

        private static IServiceCollection AddIntegrationEventHandlers(this IServiceCollection services)
        {
            var integrationEventHandlers = Application.AssemblyReference.Assembly
                .GetTypes()
                .Where(c => c.IsAssignableTo(typeof(IIntegrationEventHandler)))
                .ToArray();

            foreach (var integrationEventHandler in integrationEventHandlers)
            {
                services.TryAddTransient(integrationEventHandler);

                var integrationEvent = integrationEventHandler
                    .GetInterfaces()
                    .Single(c => c.IsGenericType)
                    .GetGenericArguments()
                    .Single();

                var closedIdempotentHandler = typeof(AttendanceIdempotentIntegrationEventHandler<>)
                    .MakeGenericType(integrationEvent);

                services.Decorate(integrationEventHandler, closedIdempotentHandler);
            }

            return services;
        }
    }
}