using Eventix.Modules.Events.Application.Events;
using Eventix.Modules.Events.Infrastructure.Database;
using Eventix.Modules.Events.Presentation.Events;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eventix.Modules.Events.Infrastructure
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
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

            services.AddDbContext<EventsDbContext>(options =>
            {
                options.UseSqlServer(connectionString);

                options.LogTo(Console.WriteLine);
            });

            return services;
        }
    }
}