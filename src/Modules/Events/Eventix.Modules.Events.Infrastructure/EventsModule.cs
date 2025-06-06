using Eventix.Modules.Events.Application.Abstractions;
using Eventix.Modules.Events.Application.Abstractions.Clock;
using Eventix.Modules.Events.Application.Abstractions.Data;
using Eventix.Modules.Events.Domain.Categories.Interfaces;
using Eventix.Modules.Events.Domain.Events.Interfaces;
using Eventix.Modules.Events.Domain.TicketTypes.Interfaces;
using Eventix.Modules.Events.Infrastructure.Categories;
using Eventix.Modules.Events.Infrastructure.Clock;
using Eventix.Modules.Events.Infrastructure.Database;
using Eventix.Modules.Events.Infrastructure.Events;
using Eventix.Modules.Events.Infrastructure.Factories;
using Eventix.Modules.Events.Infrastructure.TicketTypes;
using Eventix.Modules.Events.Presentation;
using FluentValidation;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MidR.DependencyInjection;

namespace Eventix.Modules.Events.Infrastructure
{
    public static class EventsModule
    {
        private const string DEFAULT_CONNECTION = "DefaultConnection";
        private const string CONNECTION_ERROR_MESSAGE = $"The connection string {DEFAULT_CONNECTION} is not configured";

        public static void MapEndpoints(IEndpointRouteBuilder app)
            => EventsEndpoints.MapEndpoints(app);

        public static IServiceCollection AddEventsModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMidR(AssemblyReference.Assembly);
            services.AddValidatorsFromAssembly(AssemblyReference.Assembly, includeInternalTypes: true);

            services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.AddInfrastructure(configuration);

            return services;
        }

        private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkDbContext(configuration);
            services.AddRepositories();
            services.AddSqlConnectionFactory(configuration);
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());
        }

        private static void AddSqlConnectionFactory(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>(sp =>
            {
                var connectionString = configuration.GetConnectionString(DEFAULT_CONNECTION)
                    ?? throw new InvalidOperationException(CONNECTION_ERROR_MESSAGE);

                return new SqlConnectionFactory(connectionString);
            });
        }

        private static void AddEntityFrameworkDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(DEFAULT_CONNECTION)
                ?? throw new InvalidOperationException(CONNECTION_ERROR_MESSAGE);

            services.AddDbContext<EventsDbContext>(options =>
            {
                options.UseSqlServer(connectionString);

                options.LogTo(Console.WriteLine);
            });
        }
    }
}