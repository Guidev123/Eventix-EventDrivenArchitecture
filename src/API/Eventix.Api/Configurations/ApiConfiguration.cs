using EventsAssembly = Eventix.Modules.Events.Application.AssemblyReference;
using UsersAssembly = Eventix.Modules.Users.Application.AssemblyReference;
using TicketingAssembly = Eventix.Modules.Ticketing.Application.AssemblyReference;
using Eventix.Api.Extensions;
using Eventix.Api.Middlewares;
using Eventix.Modules.Events.Infrastructure;
using Eventix.Modules.Users.Infrastructure;
using Eventix.Shared.Infrastructure;
using Serilog;
using Eventix.Modules.Ticketing.Infrastructure;

namespace Eventix.Api.Configurations
{
    public static class ApiConfiguration
    {
        private const string EVENTS_MODULE = "events";
        private const string USERS_MODULE = "users";
        private const string TICKETING_MODULE = "ticketing";

        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            var dbConnectionString = builder.Configuration.GetConnectionString("Database") ?? string.Empty;
            var redisConnectionString = builder.Configuration.GetConnectionString("Cache") ?? string.Empty;

            builder.Services.AddOpenApi();

            builder.AddSwaggerConfig();

            builder.Host.UseSerilog((context, loggerConfig)
                => loggerConfig.ReadFrom.Configuration(context.Configuration));

            builder.AddExceptionHandler();

            builder.AddCustomHealthChecks(dbConnectionString, redisConnectionString);

            builder.Services.AddInfrastructure([TicketingModule.ConfigureConsumers], dbConnectionString, redisConnectionString);

            builder.AddAllModules();

            builder.Services.AddApplication([
                EventsAssembly.Assembly,
                UsersAssembly.Assembly,
                TicketingAssembly.Assembly
                ]);

            return builder;
        }

        private static WebApplicationBuilder AddExceptionHandler(this WebApplicationBuilder builder)
        {
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            return builder;
        }

        private static WebApplicationBuilder AddCustomHealthChecks(
            this WebApplicationBuilder builder,
            string dbConnectionString,
            string redisConnectionString
            )
        {
            var appSettingsSection = builder.Configuration.GetSection(nameof(KeyCloakExtensions));
            var appSettings = appSettingsSection.Get<KeyCloakExtensions>()
                ?? throw new InvalidOperationException("Keycloak settings not found.");

            builder.Services.AddHealthChecks()
                .AddSqlServer(dbConnectionString)
                .AddRedis(redisConnectionString)
                .AddUrlGroup(new Uri(appSettings.HealthUrl), HttpMethod.Get, "keycloak");

            return builder;
        }

        private static WebApplicationBuilder AddAllModules(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddEventsModule(builder.Configuration)
                .AddUsersModule(builder.Configuration)
                .AddTicketingModule(builder.Configuration);

            builder.Configuration.AddModuleConfiguration([
                EVENTS_MODULE,
                USERS_MODULE,
                TICKETING_MODULE
                ]);

            return builder;
        }
    }
}