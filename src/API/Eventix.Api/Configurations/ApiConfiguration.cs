﻿using Eventix.Api.Extensions;
using Eventix.Api.Middlewares;
using Eventix.Api.OpenTelemetry;
using Eventix.Modules.Attendance.Infrastructure;
using Eventix.Modules.Events.Infrastructure;
using Eventix.Modules.Ticketing.Infrastructure;
using Eventix.Modules.Users.Infrastructure;
using Eventix.Shared.Infrastructure;
using Eventix.Shared.Infrastructure.EventBus;
using HealthChecks.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using RabbitMQ.Client;
using Serilog;
using AttendanceAssembly = Eventix.Modules.Attendance.Application.AssemblyReference;
using BrokerOptions = Eventix.Shared.Infrastructure.EventBus.BrokerOptions;
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
            var eventStoreString = builder.Configuration.GetConnectionString("EventStore") ?? string.Empty;

            builder.Services.AddOpenApi();

            builder.AddSwaggerConfig();

            builder.AddExceptionHandler();

            builder.AddAllModules(dbConnectionString, redisConnectionString);

            if (!builder.Environment.IsEnvironment("Testing"))
            {
                builder.Host.UseSerilog((context, loggerConfig)
                    => loggerConfig.ReadFrom.Configuration(context.Configuration));

                builder.AddCustomHealthChecks(dbConnectionString, redisConnectionString, eventStoreString);

                builder.Services.AddTracing(builder.Configuration, DiagnosticConfig.ServiceName);
            }

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
            string redisConnectionString,
            string eventStoreString
        )
        {
            var keycloakOptions = builder.Configuration.GetSection(nameof(KeyCloakExtensions)).Get<KeyCloakExtensions>()
                ?? throw new InvalidOperationException("Keycloak settings not found");

            var brokerOptions = builder.Configuration.GetSection(nameof(BrokerOptions)).Get<BrokerOptions>() ?? new();

            builder.Services
                .AddHealthChecks()
                .AddSqlServer(dbConnectionString)
                .AddEventStore(eventStoreString)
                .AddRedis(redisConnectionString)
                .AddRabbitMQ(sp =>
                {
                    var factory = new ConnectionFactory
                    {
                        UserName = brokerOptions.Username,
                        Password = brokerOptions.Password,
                        VirtualHost = brokerOptions.VirtualHost,
                        AutomaticRecoveryEnabled = true
                    };

                    var endpoints = brokerOptions.Hosts.Select(host => new AmqpTcpEndpoint(host)).ToList();

                    return factory.CreateConnectionAsync(endpoints);
                })
                .AddUrlGroup(new Uri(keycloakOptions.HealthUrl), HttpMethod.Get, "keycloak");

            return builder;
        }

        private static WebApplicationBuilder AddAllModules(
            this WebApplicationBuilder builder,
            string dbConnectionString,
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
                .AddInfrastructure(builder.Configuration, dbConnectionString, redisConnectionString)
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