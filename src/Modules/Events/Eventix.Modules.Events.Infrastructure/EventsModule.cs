using Eventix.Modules.Events.Domain.Categories.Interfaces;
using Eventix.Modules.Events.Domain.Events.Interfaces;
using Eventix.Modules.Events.Domain.Shared.Interfaces;
using Eventix.Modules.Events.Domain.TicketTypes.Interfaces;
using Eventix.Modules.Events.Infrastructure.Categories;
using Eventix.Modules.Events.Infrastructure.Database;
using Eventix.Modules.Events.Infrastructure.Events;
using Eventix.Modules.Events.Infrastructure.TicketTypes;
using Eventix.Modules.Events.Presentation;
using Eventix.Shared.Infrastructure.Interceptors;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eventix.Modules.Events.Infrastructure
{
    public static class EventsModule
    {
        private const string DEFAULT_CONNECTION = "DefaultConnection";
        private const string CONNECTION_ERROR_MESSAGE = $"The connection string {DEFAULT_CONNECTION} is not configured";

        public static IServiceCollection AddEventsModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEndpoints(typeof(PresentationModule).Assembly);

            services.AddEntityFrameworkDbContext(configuration);
            services.AddRepositories();
            return services;
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());
        }

        private static void AddEntityFrameworkDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(DEFAULT_CONNECTION)
                ?? throw new InvalidOperationException(CONNECTION_ERROR_MESSAGE);

            services.AddDbContext<EventsDbContext>((sp, options) =>
            {
                var publishDomainEventsInterceptor = sp.GetRequiredService<PublishDomainEventsInterceptors>();

                options.UseSqlServer(connectionString).AddInterceptors(publishDomainEventsInterceptor).LogTo(Console.WriteLine);
            });
        }
    }
}