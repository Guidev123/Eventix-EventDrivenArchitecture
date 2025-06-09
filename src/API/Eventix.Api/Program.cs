using Eventix.Api.Configurations;
using Eventix.Api.Extensions;
using Eventix.Api.Middlewares;
using Eventix.Modules.Events.Application;
using Eventix.Modules.Events.Infrastructure;
using Eventix.Shared.Infrastructure;
using Eventix.Shared.Presentation.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

var dbConnectionString = builder.Configuration.GetConnectionString("Database") ?? string.Empty;
var redisConnectionString = builder.Configuration.GetConnectionString("Cache") ?? string.Empty;

builder.Services.AddHealthChecks()
    .AddSqlServer(dbConnectionString)
    .AddRedis(redisConnectionString);

builder.AddSwaggerConfig();
builder.Services.AddEventsModule(builder.Configuration);

builder.Services.AddApplication([AssemblyReference.Assembly]);
builder.Services.AddInfrastructure(dbConnectionString, redisConnectionString);
builder.Configuration.AddModuleConfiguration(["events"]);
var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerConfig();

    app.ApplyMigrations();
}

app.MapEndpoints();

app.MapHealthChecks("healthz", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseSerilogRequestLogging();

app.Run();