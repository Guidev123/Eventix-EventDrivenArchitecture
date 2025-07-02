using Eventix.Modules.Events.Domain.Categories.Interfaces;
using Eventix.Modules.Events.Domain.Events.Interfaces;
using Eventix.Modules.Events.Domain.TicketTypes.Interfaces;
using Eventix.Modules.Events.Infrastructure.Categories.Repositories;
using Eventix.Modules.Events.Infrastructure.Database;
using Eventix.Modules.Events.Infrastructure.Events.Repositories;
using Eventix.Modules.Events.Infrastructure.TicketTypes.Repositories;
using Eventix.Modules.Events.Presentation;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Infrastructure.Outbox.Interceptors;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Eventix.Modules.Events.Infrastructure
{
    public static class EventsModule
    {
        private const string DATABASE_CONNECTION = "Database";
        private const string CONNECTION_ERROR_MESSAGE = $"The connection string {DATABASE_CONNECTION} is not configured";

        public static IServiceCollection AddEventsModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEndpoints(typeof(PresentationModule).Assembly);

            AddDomainEventHandlers(services);
            AddRepositories(services);
            AddEntityFrameworkDbContext(services, configuration);

            return services;
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
        }

        private static void AddEntityFrameworkDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(DATABASE_CONNECTION)
                ?? throw new InvalidOperationException(CONNECTION_ERROR_MESSAGE);

            services.AddDbContext<EventsDbContext>((sp, options) =>
            {
                var publishDomainEventsInterceptor = sp.GetRequiredService<InsertOutboxMessagesInterceptors>();

                options.UseSqlServer(connectionString).AddInterceptors(publishDomainEventsInterceptor).LogTo(Console.WriteLine);
            });
        }

        private static void AddDomainEventHandlers(this IServiceCollection services)
        {
            var domainEventHandlers = Application.AssemblyReference.Assembly
                .GetTypes()
                .Where(c => c.IsAssignableTo(typeof(IDomainEventHandler)))
                .ToArray();

            foreach (var domainEventHandler in domainEventHandlers)
            {
                services.TryAddTransient(domainEventHandler);
            }
        }
    }
}