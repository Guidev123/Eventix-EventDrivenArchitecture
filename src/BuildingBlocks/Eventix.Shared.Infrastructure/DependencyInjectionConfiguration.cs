using Eventix.Shared.Application.Cache;
using Eventix.Shared.Application.Clock;
using Eventix.Shared.Application.Decorators;
using Eventix.Shared.Application.Factories;
using Eventix.Shared.Infrastructure.Cache;
using Eventix.Shared.Infrastructure.Clock;
using Eventix.Shared.Infrastructure.Factories;
using Eventix.Shared.Infrastructure.Interceptors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MidR.DependencyInjection;
using MidR.Interfaces;
using StackExchange.Redis;
using System.Reflection;

namespace Eventix.Shared.Infrastructure
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            string databaseConnectionString,
            string redisConnectionString)
        {
            AddData(services, databaseConnectionString)
                .AddCacheService(redisConnectionString);

            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services, Assembly[] modulesAssemblies)
        {
            AddHandlers(services, modulesAssemblies);
            services.AddValidatorsFromAssemblies(modulesAssemblies, includeInternalTypes: true);
            services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

            return services;
        }

        private static IServiceCollection AddData(this IServiceCollection services, string databaseConnectionString)
        {
            services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>(sp =>
            {
                return new SqlConnectionFactory(databaseConnectionString);
            });

            services.TryAddSingleton<PublishDomainEventsInterceptors>();

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
                services.AddDistributedMemoryCache();
            }

            return services;
        }
    }
}