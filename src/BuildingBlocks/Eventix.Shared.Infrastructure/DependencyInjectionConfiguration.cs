using Eventix.Shared.Application.Abstractions;
using Eventix.Shared.Application.Cache;
using Eventix.Shared.Application.Clock;
using Eventix.Shared.Application.Decorators;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.EventSourcing;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Infrastructure.Authentication;
using Eventix.Shared.Infrastructure.Authorization;
using Eventix.Shared.Infrastructure.Cache;
using Eventix.Shared.Infrastructure.Clock;
using Eventix.Shared.Infrastructure.EventBus;
using Eventix.Shared.Infrastructure.EventSourcing;
using Eventix.Shared.Infrastructure.Factories;
using Eventix.Shared.Infrastructure.Outbox.Interceptors;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using MidR.DependencyInjection;
using MidR.Interfaces;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Quartz;
using StackExchange.Redis;
using System.Reflection;

namespace Eventix.Shared.Infrastructure
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration,
            string databaseConnectionString,
            string redisConnectionString)
        {
            services
                .AddAuthenticationInternal()
                .AddAuthorizationInternal()
                .AddEventSourcing()
                .AddData(databaseConnectionString)
                .AddCacheService(redisConnectionString)
                .AddBus(configuration)
                .AddBackgroundJobs();

            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services, Assembly[] modulesAssemblies)
        {
            services.AddHandlers(modulesAssemblies);
            services.AddValidatorsFromAssemblies(modulesAssemblies, includeInternalTypes: true);
            services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

            return services;
        }

        private static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
        {
            services.AddQuartz(c =>
            {
                var schedulerId = Guid.NewGuid();
                c.SchedulerId = $"default-id-{schedulerId}";
                c.SchedulerName = $"dafault-name-{schedulerId}";
            });

            services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

            return services;
        }

        private static IServiceCollection AddData(this IServiceCollection services, string databaseConnectionString)
        {
            services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>(sp =>
            {
                return new SqlConnectionFactory(databaseConnectionString);
            });

            services.TryAddSingleton<InsertOutboxMessagesInterceptors>();

            return services;
        }

        private static IServiceCollection AddHandlers(this IServiceCollection services, Assembly[] modulesAssemblies)
        {
            services
                .AddMidR(modulesAssemblies)
                .AddRequestHandlerDecorators();

            services.TryAddScoped<IMediatorHandler, MediatorHandler>();

            return services;
        }

        private static IServiceCollection AddRequestHandlerDecorators(this IServiceCollection services)
        {
            services.Decorate(typeof(IRequestHandler<,>), typeof(ValidationDecorator.RequestHandler<,>));
            services.Decorate(typeof(IRequestHandler<,>), typeof(RequestLoggingDecorator.RequestHandler<,>));

            return services;
        }

        private static IServiceCollection AddCacheService(this IServiceCollection services, string redisConnectionString)
        {
            try
            {
                IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
                services.TryAddSingleton(connectionMultiplexer);

                services.AddStackExchangeRedisCache(options =>
                {
                    options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer);
                });

                services.TryAddSingleton<ICacheService, CacheService>();
            }
            catch
            {
                services.TryAddSingleton<ICacheService, CacheService>();
                services.AddDistributedMemoryCache();
            }

            return services;
        }

        private static IServiceCollection AddBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddSingleton<IBusFailureHandlingService, BusFailureHandlingService>();

            services.TryAddSingleton<IBus>(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<Bus>>();
                var busFailureHandlingService = serviceProvider.GetRequiredService<IBusFailureHandlingService>();
                var brokerOptions = configuration.GetSection(nameof(BrokerOptions)).Get<BrokerOptions>() ?? new();

                return new Bus(options =>
                {
                    options.Username = brokerOptions.Username;
                    options.Password = brokerOptions.Password;
                    options.VirtualHost = brokerOptions.VirtualHost;
                    options.Hosts = brokerOptions.Hosts;
                }, logger, busFailureHandlingService);
            });

            return services;
        }

        private static IServiceCollection AddEventSourcing(this IServiceCollection services)
        {
            services.AddSingleton<IEventStoreService, EventStoreService>();
            services.AddSingleton<IEventSourcingRepository, EventSourcingRepository>();

            return services;
        }

        public static IServiceCollection AddTracing(this IServiceCollection services, IConfiguration configuration, string serviceName)
        {
            services
                .AddOpenTelemetry()
                .ConfigureResource(c => c.AddService(serviceName))
                .WithTracing(tracing =>
                {
                    tracing
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddSqlClientInstrumentation()
                        .AddEntityFrameworkCoreInstrumentation()
                        .AddRabbitMQInstrumentation()
                        .AddRedisInstrumentation();

                    tracing.AddOtlpExporter();
                });

            return services;
        }
    }
}