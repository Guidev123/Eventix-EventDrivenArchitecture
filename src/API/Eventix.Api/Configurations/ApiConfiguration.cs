using Eventix.Api.Extensions;
using Eventix.Api.Middlewares;
using Eventix.Modules.Events.Application;
using Eventix.Modules.Events.Infrastructure;
using Eventix.Modules.Users.Infrastructure;
using Eventix.Shared.Infrastructure;
using Serilog;

namespace Eventix.Api.Configurations
{
    public static class ApiConfiguration
    {
        private const string EVENTS_MODULE = "events";
        private const string USERS_MODULE = "users";

        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            var dbConnectionString = builder.Configuration.GetConnectionString("Database") ?? string.Empty;
            var redisConnectionString = builder.Configuration.GetConnectionString("Cache") ?? string.Empty;

            builder.Services.AddOpenApi();
            builder.AddSwaggerConfig();
            builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));
            builder.AddExceptionHandler();
            builder.AddCustomHealthChecks(dbConnectionString, redisConnectionString);
            builder.AddAllModules();
            builder.Services.AddApplication([AssemblyReference.Assembly]);
            builder.Services.AddInfrastructure(dbConnectionString, redisConnectionString);

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
            builder.Services.AddHealthChecks()
                .AddSqlServer(dbConnectionString)
                .AddRedis(redisConnectionString);

            return builder;
        }

        private static WebApplicationBuilder AddAllModules(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddEventsModule(builder.Configuration)
                .AddUsersModule(builder.Configuration);

            builder.Configuration.AddModuleConfiguration([EVENTS_MODULE, USERS_MODULE]);

            return builder;
        }
    }
}