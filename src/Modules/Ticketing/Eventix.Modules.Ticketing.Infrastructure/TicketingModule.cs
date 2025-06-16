using Eventix.Modules.Ticketing.Application.Carts.Services;
using Eventix.Modules.Ticketing.Infrastructure.PublicApi;
using Eventix.Modules.Ticketing.Presentation;
using Eventix.Modules.Ticketing.PublicApi;
using Eventix.Shared.Infrastructure.Interceptors;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eventix.Modules.Ticketing.Infrastructure
{
    public static class TicketingModule
    {
        private const string DATABASE_CONNECTION = "Database";
        private const string CONNECTION_ERROR_MESSAGE = $"The connection string {DATABASE_CONNECTION} is not configured";

        public static IServiceCollection AddTicketingModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEndpoints(typeof(PresentationModule).Assembly);
            services.AddTransient<ITicketingApi, TicketingApi>();

            AddServices(services);
            AddEntityFrameworkDbContext(services, configuration);

            return services;
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ICartService, CartService>();
        }

        private static void AddEntityFrameworkDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(DATABASE_CONNECTION)
                ?? throw new InvalidOperationException(CONNECTION_ERROR_MESSAGE);

            services.AddDbContext<DbContext>((sp, options) =>
            {
                var publishDomainEventsInterceptor = sp.GetRequiredService<PublishDomainEventsInterceptors>();

                options.UseSqlServer(connectionString).AddInterceptors(publishDomainEventsInterceptor).LogTo(Console.WriteLine);
            });
        }
    }
}