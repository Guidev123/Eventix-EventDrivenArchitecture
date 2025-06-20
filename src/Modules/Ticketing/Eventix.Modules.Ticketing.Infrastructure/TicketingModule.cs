using Eventix.Modules.Ticketing.Application.Abstractions.Authentication;
using Eventix.Modules.Ticketing.Application.Abstractions.Services;
using Eventix.Modules.Ticketing.Application.Carts.Services;
using Eventix.Modules.Ticketing.Domain.Customers.Interfaces;
using Eventix.Modules.Ticketing.Domain.Events.Interfaces;
using Eventix.Modules.Ticketing.Domain.Orders.Interfaces;
using Eventix.Modules.Ticketing.Domain.Payments.Interfaces;
using Eventix.Modules.Ticketing.Domain.Tickets.Interfaces;
using Eventix.Modules.Ticketing.Infrastructure.Authentication;
using Eventix.Modules.Ticketing.Infrastructure.Customers.Consumers;
using Eventix.Modules.Ticketing.Infrastructure.Customers.Repositories;
using Eventix.Modules.Ticketing.Infrastructure.Database;
using Eventix.Modules.Ticketing.Infrastructure.Events.Repositories;
using Eventix.Modules.Ticketing.Infrastructure.Orders.Repositories;
using Eventix.Modules.Ticketing.Infrastructure.Payments.Repositories;
using Eventix.Modules.Ticketing.Infrastructure.Payments.Services;
using Eventix.Modules.Ticketing.Infrastructure.Tickets.Repositories;
using Eventix.Modules.Ticketing.Presentation;
using Eventix.Shared.Domain.Interfaces;
using Eventix.Shared.Infrastructure.Interceptors;
using Eventix.Shared.Presentation.Extensions;
using MassTransit;
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

            AddRepositories(services);
            AddServices(services);
            AddEntityFrameworkDbContext(services, configuration);

            return services;
        }

        public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator)
        {
            registrationConfigurator.AddConsumer<UserRegisteredIntegrationEventConsumer>();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ICartService, CartService>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddScoped<ICustomerContext, CustomerContext>();
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<TicketingDbContext>());
        }

        private static void AddEntityFrameworkDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(DATABASE_CONNECTION)
                ?? throw new InvalidOperationException(CONNECTION_ERROR_MESSAGE);

            services.AddDbContext<TicketingDbContext>((sp, options) =>
            {
                var publishDomainEventsInterceptor = sp.GetRequiredService<PublishDomainEventsInterceptors>();

                options.UseSqlServer(connectionString).AddInterceptors(publishDomainEventsInterceptor).LogTo(Console.WriteLine);
            });
        }
    }
}