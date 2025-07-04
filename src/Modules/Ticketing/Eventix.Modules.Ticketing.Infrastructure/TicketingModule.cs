using Eventix.Modules.Ticketing.Application.Abstractions.Services;
using Eventix.Modules.Ticketing.Application.Carts.Services;
using Eventix.Modules.Ticketing.Domain.Customers.Interfaces;
using Eventix.Modules.Ticketing.Domain.Events.Interfaces;
using Eventix.Modules.Ticketing.Domain.Orders.Interfaces;
using Eventix.Modules.Ticketing.Domain.Payments.Interfaces;
using Eventix.Modules.Ticketing.Domain.Tickets.Interfaces;
using Eventix.Modules.Ticketing.Infrastructure.Customers.IntegrationEventHandlers;
using Eventix.Modules.Ticketing.Infrastructure.Customers.Repositories;
using Eventix.Modules.Ticketing.Infrastructure.Database;
using Eventix.Modules.Ticketing.Infrastructure.Events.Repositories;
using Eventix.Modules.Ticketing.Infrastructure.Inbox;
using Eventix.Modules.Ticketing.Infrastructure.Orders.Repositories;
using Eventix.Modules.Ticketing.Infrastructure.Outbox;
using Eventix.Modules.Ticketing.Infrastructure.Payments.Repositories;
using Eventix.Modules.Ticketing.Infrastructure.Payments.Services;
using Eventix.Modules.Ticketing.Infrastructure.Tickets.Repositories;
using Eventix.Modules.Ticketing.Presentation;
using Eventix.Modules.Users.IntegrationEvents.Users;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Infrastructure.Inbox;
using Eventix.Shared.Infrastructure.Outbox.Interceptors;
using Eventix.Shared.Presentation.Extensions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Eventix.Modules.Ticketing.Infrastructure
{
    public static class TicketingModule
    {
        private const string DATABASE_CONNECTION = "Database";
        private const string CONNECTION_ERROR_MESSAGE = $"The connection string {DATABASE_CONNECTION} is not configured";

        public static IServiceCollection AddTicketingModule(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddEndpoints(typeof(PresentationModule).Assembly)
                .AddServices()
                .AddRepositories()
                .AddDomainEventHandlers()
                .AddIntegrationEventHandlers()
                .AddInbox(configuration)
                .AddOutbox(configuration)
                .AddEntityFrameworkDbContext(configuration);

            return services;
        }

        public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator)
        {
            registrationConfigurator.AddConsumer<IntegrationEventConsumer<UserRegisteredIntegrationEvent>>();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ICartService, CartService>();
            services.AddTransient<IPaymentService, PaymentService>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IEventRepository, EventRepository>();

            return services;
        }

        private static IServiceCollection AddEntityFrameworkDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(DATABASE_CONNECTION)
                ?? throw new InvalidOperationException(CONNECTION_ERROR_MESSAGE);

            services.AddDbContext<TicketingDbContext>((sp, options) =>
            {
                var insertOutboxMessagesInterceptor = sp.GetRequiredService<InsertOutboxMessagesInterceptors>();

                options.UseSqlServer(connectionString).AddInterceptors(insertOutboxMessagesInterceptor).LogTo(Console.WriteLine);
            });

            return services;
        }

        private static IServiceCollection AddOutbox(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OutboxOptions>(configuration.GetSection("Ticketing:Outbox"));
            services.ConfigureOptions<ConfigureProcessOutboxJob>();

            return services;
        }

        private static IServiceCollection AddInbox(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<InboxOptions>(configuration.GetSection("Ticketing:Inbox"));
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

                var closedIdempotentHandler = typeof(TicketingIdempotentDomainEventHandler<>)
                    .MakeGenericType(domainEvent);

                services.Decorate(domainEventHandler, closedIdempotentHandler);
            }

            return services;
        }

        public static IServiceCollection AddIntegrationEventHandlers(this IServiceCollection services)
        {
            var integrationEventHandlers = typeof(TicketingModule).Assembly.GetTypes()
             .Where(c => c.IsAssignableTo(typeof(IIntegrationEventHandler)) && !c.IsAbstract && !c.IsInterface)
             .Where(c => !c.Name.Contains(nameof(IdempotentIntegrationEventHandler<IntegrationEvent>)))
             .Where(c => !c.IsGenericTypeDefinition)
             .Where(c => c.IsClass && !c.IsAbstract)
             .ToArray();

            foreach (var integrationEventHandler in integrationEventHandlers)
            {
                services.TryAddTransient(integrationEventHandler);

                var integrationEvent = integrationEventHandler
                    .GetInterfaces()
                    .Single(c => c.IsGenericType && c.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>))
                    .GetGenericArguments()
                    .Single();

                var closedIdempotentHandler = typeof(TicketingIdempotentIntegrationEventHandler<>)
                    .MakeGenericType(integrationEvent);

                services.Decorate(integrationEventHandler, closedIdempotentHandler);
            }

            return services;
        }
    }
}