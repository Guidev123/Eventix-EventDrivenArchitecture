using Eventix.Shared.Application.Cache;
using Eventix.Shared.Application.Clock;
using Eventix.Shared.Application.Decorators;
using Eventix.Shared.Application.EventBus;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Infrastructure.Authentication;
using Eventix.Shared.Infrastructure.Authorization;
using Eventix.Shared.Infrastructure.Cache;
using Eventix.Shared.Infrastructure.Clock;
using Eventix.Shared.Infrastructure.Factories;
using Eventix.Shared.Infrastructure.Outbox.Interceptors;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MidR.DependencyInjection;
using MidR.Interfaces;
using Quartz;
using StackExchange.Redis;
using System.Reflection;

namespace Eventix.Shared.Infrastructure
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            Action<IRegistrationConfigurator>[] moduleConfigureConsumers,
            string databaseConnectionString,
            string redisConnectionString)
        {
            services
                .AddAuthenticationInternal()
                .AddAuthorizationInternal()
                .AddData(databaseConnectionString)
                .AddCacheService(redisConnectionString)
                .AddBus(moduleConfigureConsumers)
                .AddBackgroundJobs();

            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services, Assembly[] modulesAssemblies)
        {
            AddHandlers(services, modulesAssemblies);
            services.AddValidatorsFromAssemblies(modulesAssemblies, includeInternalTypes: true);
            services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

            return services;
        }

        private static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
        {
            services.AddQuartz();

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
            services.AddMidR(modulesAssemblies);
            AddRequestHandlerDecorators(services);

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

        private static IServiceCollection AddBus(this IServiceCollection services, Action<IRegistrationConfigurator>[] moduleConfigureConsumers)
        {
            services.TryAddSingleton<IEventBus, EventBus.EventBus>();
            services.AddMassTransit(opt =>
            {
                foreach (var configureConsumer in moduleConfigureConsumers)
                {
                    configureConsumer(opt);
                }

                opt.SetKebabCaseEndpointNameFormatter();

                opt.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}