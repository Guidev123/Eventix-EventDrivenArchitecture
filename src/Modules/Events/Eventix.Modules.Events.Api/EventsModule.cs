using Eventix.Modules.Events.Api.Database;
using Eventix.Modules.Events.Api.Events;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eventix.Modules.Events.Api
{
    public static class EventsModule
    {
        public static void MapEndpoints(IEndpointRouteBuilder app)
        {
            CreateEvent.MapEndpoint(app);
            GetEvent.MapEndpoint(app);
        }

        public static IServiceCollection AddEventsModule(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

            services.AddDbContext<EventsDbContext>(options =>
            {
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Events);
                });

                options.LogTo(Console.WriteLine);
            });

            return services;
        }
    }
}