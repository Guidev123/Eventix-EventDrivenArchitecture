using Eventix.Shared.Application.Clock;
using Eventix.Shared.Application.Data;
using Eventix.Shared.Infrastructure.Clock;
using Eventix.Shared.Infrastructure.Factories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MidR.DependencyInjection;
using System.Reflection;

namespace Eventix.Shared.Infrastructure
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string databaseConnectionString)
        {
            services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>(sp =>
            {
                return new SqlConnectionFactory(databaseConnectionString);
            });

            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services, Assembly[] modulesAssemblies)
        {
            services.AddMidR(modulesAssemblies);
            services.AddValidatorsFromAssemblies(modulesAssemblies, includeInternalTypes: true);
            services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

            return services;
        }
    }
}