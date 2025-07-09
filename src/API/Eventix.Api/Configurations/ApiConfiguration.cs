using Eventix.Api.Extensions;
using Eventix.Api.Middlewares;
using Eventix.Modules.Attendance.Infrastructure;
using Eventix.Modules.Events.Infrastructure;
using Eventix.Modules.Ticketing.Infrastructure;
using Eventix.Modules.Users.Infrastructure;
using Eventix.Shared.Infrastructure;
using HealthChecks.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Serilog;
using AttendanceAssembly = Eventix.Modules.Attendance.Application.AssemblyReference;
using EventsAssembly = Eventix.Modules.Events.Application.AssemblyReference;
using TicketingAssembly = Eventix.Modules.Ticketing.Application.AssemblyReference;
using UsersAssembly = Eventix.Modules.Users.Application.AssemblyReference;

namespace Eventix.Api.Configurations
{
    public static class ApiConfiguration
    {
        private const string EVENTS_MODULE = "events";
        private const string USERS_MODULE = "users";
        private const string TICKETING_MODULE = "ticketing";
        private const string ATTENDANCE_MODULE = "attendance";

        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            var dbConnectionString = builder.Configuration.GetConnectionString("Database") ?? string.Empty;
            var redisConnectionString = builder.Configuration.GetConnectionString("Cache") ?? string.Empty;
            var messageBusConnectionString = builder.Configuration.GetConnectionString("MessageBus") ?? string.Empty;

            builder.Services.AddOpenApi();

            builder.AddSwaggerConfig();

            builder.Host.UseSerilog((context, loggerConfig)
                => loggerConfig.ReadFrom.Configuration(context.Configuration));

            builder.AddExceptionHandler();

            builder.AddCustomHealthChecks(dbConnectionString, messageBusConnectionString, redisConnectionString);

            builder.AddAllModules(dbConnectionString, messageBusConnectionString, redisConnectionString);

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
            string messageBusConnectionString,
            string redisConnectionString
        )
        {
            var appSettingsSection = builder.Configuration.GetSection(nameof(KeyCloakExtensions));
            var appSettings = appSettingsSection.Get<KeyCloakExtensions>()
                ?? throw new InvalidOperationException("Keycloak settings not found.");

            builder.Services
                .AddHealthChecks()
                .AddSqlServer(dbConnectionString)
                .AddRedis(redisConnectionString)
                .AddRabbitMQ(sp =>
                {
                    var factory = new ConnectionFactory
                    {
                        Uri = new Uri(messageBusConnectionString),
                        AutomaticRecoveryEnabled = true
                    };

                    return factory.CreateConnectionAsync();
                })
                .AddUrlGroup(new Uri(appSettings.HealthUrl), HttpMethod.Get, "keycloak");

            return builder;
        }

        private static WebApplicationBuilder AddAllModules(
            this WebApplicationBuilder builder,
            string dbConnectionString,
            string messageBusConnectionString,
            string redisConnectionString
            )
        {
            builder.Services
                .AddApplication([
                    EventsAssembly.Assembly,
                    UsersAssembly.Assembly,
                    TicketingAssembly.Assembly,
                    AttendanceAssembly.Assembly
                ])
                .AddInfrastructure(messageBusConnectionString, dbConnectionString, redisConnectionString)
                .AddTicketingModule(builder.Configuration)
                .AddEventsModule(builder.Configuration)
                .AddUsersModule(builder.Configuration)
                .AddAttendanceModule(builder.Configuration);

            builder.Configuration.AddModuleConfiguration([
                EVENTS_MODULE,
                USERS_MODULE,
                TICKETING_MODULE,
                ATTENDANCE_MODULE
                ]);

            return builder;
        }
    }
}