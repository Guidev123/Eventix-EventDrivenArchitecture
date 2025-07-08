using Eventix.Modules.Users.Application.Abstractions.Identity.Services;
using Eventix.Modules.Users.Domain.Users.Interfaces;
using Eventix.Modules.Users.Infrastructure.Authorization;
using Eventix.Modules.Users.Infrastructure.Database;
using Eventix.Modules.Users.Infrastructure.Identity;
using Eventix.Modules.Users.Infrastructure.Inbox;
using Eventix.Modules.Users.Infrastructure.Outbox;
using Eventix.Modules.Users.Infrastructure.Users.Repositories;
using Eventix.Modules.Users.Presentation;
using Eventix.Shared.Application.Authorization;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Infrastructure.Http;
using Eventix.Shared.Infrastructure.Inbox;
using Eventix.Shared.Infrastructure.Outbox.Interceptors;
using Eventix.Shared.Presentation.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Eventix.Modules.Users.Infrastructure
{
    public static class UsersModule
    {
        private const string DATABASE_CONNECTION = "Database";
        private const string CONNECTION_ERROR_MESSAGE = $"The connection string {DATABASE_CONNECTION} is not configured";

        public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddEndpoints(typeof(PresentationModule).Assembly)
                .AddServices()
                .AddRepositories()
                .AddDomainEventHandlers()
                .AddIntegrationEventHandlers()
                .AddInbox(configuration)
                .AddOutbox(configuration)
                .AddHttpClientServices(configuration)
                .AddEntityFrameworkDbContext(configuration);

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.TryAddScoped<IPermissionService, PermissionService>();
            services.AddHostedService<IntegrationEventConsumer>();

            return services;
        }

        private static IServiceCollection AddHttpClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KeyCloakOptions>(configuration.GetSection("Users:KeyCloak"));

            services.AddTransient<KeyCloakAuthDelegatingHandler>();

            services.AddHttpClient<KeyCloakClient>((serviceProvider, httpClient) =>
            {
                var keyCloakOptions = serviceProvider.GetRequiredService<IOptions<KeyCloakOptions>>().Value;

                httpClient.BaseAddress = new Uri(keyCloakOptions.AdminUrl);
            }).AddHttpMessageHandler<KeyCloakAuthDelegatingHandler>()
            .ConfigurePrimaryHttpMessageHandler(HttpMessageHandlerFactory.CreateSocketsHttpHandler)
            .SetHandlerLifetime(Timeout.InfiniteTimeSpan)
            .AddResilienceHandler(nameof(ResiliencePipelineExtensions), pipeline => pipeline.ConfigureResilience());

            services.AddTransient<IIdentityProviderService, IdentityProviderService>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        private static IServiceCollection AddEntityFrameworkDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(DATABASE_CONNECTION)
                ?? throw new InvalidOperationException(CONNECTION_ERROR_MESSAGE);

            services.AddDbContext<UsersDbContext>((sp, options) =>
            {
                var insertOutboxMessagesInterceptor = sp.GetRequiredService<InsertOutboxMessagesInterceptors>();

                options.UseSqlServer(connectionString).AddInterceptors(insertOutboxMessagesInterceptor);
            });

            return services;
        }

        private static IServiceCollection AddOutbox(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OutboxOptions>(configuration.GetSection("Users:Outbox"));
            services.ConfigureOptions<ConfigureProcessOutboxJob>();

            return services;
        }

        private static IServiceCollection AddInbox(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<InboxOptions>(configuration.GetSection("Users:Inbox"));
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

                var closedIdempotentHandler = typeof(Outbox.UsersIdempotentDomainEventHandler<>)
                    .MakeGenericType(domainEvent);

                services.Decorate(domainEventHandler, closedIdempotentHandler);
            }

            return services;
        }

        private static IServiceCollection AddIntegrationEventHandlers(this IServiceCollection services)
        {
            var integrationEventHandlers = typeof(UsersModule).Assembly.GetTypes()
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

                var closedIdempotentHandler = typeof(UsersIdempotentIntegrationEventHandler<>)
                    .MakeGenericType(integrationEvent);

                services.Decorate(integrationEventHandler, closedIdempotentHandler);
            }

            return services;
        }
    }
}