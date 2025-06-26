using Eventix.Modules.Users.Application.Abstractions.Identity.Services;
using Eventix.Modules.Users.Domain.Users.Interfaces;
using Eventix.Modules.Users.Infrastructure.Authorization;
using Eventix.Modules.Users.Infrastructure.Database;
using Eventix.Modules.Users.Infrastructure.Identity;
using Eventix.Modules.Users.Infrastructure.Users.Repositories;
using Eventix.Modules.Users.Presentation;
using Eventix.Shared.Application.Authorization;
using Eventix.Shared.Domain.Interfaces;
using Eventix.Shared.Infrastructure.Interceptors;
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
            services.AddEndpoints(typeof(PresentationModule).Assembly);

            AddServices(services);
            AddHttpClientServices(services, configuration);
            AddRepositories(services);
            AddEntityFrameworkDbContext(services, configuration);

            return services;
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.TryAddScoped<IPermissionService, PermissionService>();
        }

        private static void AddHttpClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KeyCloakOptions>(configuration.GetSection("Users:KeyCloak"));

            services.AddTransient<KeyCloakAuthDelegatingHandler>();

            services.AddHttpClient<KeyCloakClient>((serviceProvider, httpClient) =>
            {
                var keyCloakOptions = serviceProvider.GetRequiredService<IOptions<KeyCloakOptions>>().Value;

                httpClient.BaseAddress = new Uri(keyCloakOptions.AdminUrl);
            }).AddHttpMessageHandler<KeyCloakAuthDelegatingHandler>();

            services.AddTransient<IIdentityProviderService, IdentityProviderService>();
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }

        private static void AddEntityFrameworkDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(DATABASE_CONNECTION)
                ?? throw new InvalidOperationException(CONNECTION_ERROR_MESSAGE);

            services.AddDbContext<UsersDbContext>((sp, options) =>
            {
                var publishDomainEventsInterceptor = sp.GetRequiredService<PublishDomainEventsInterceptors>();

                options.UseSqlServer(connectionString).AddInterceptors(publishDomainEventsInterceptor).LogTo(Console.WriteLine);
            });
        }
    }
}