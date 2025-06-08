using Eventix.Api.Configurations;
using Eventix.Api.Extensions;
using Eventix.Api.Middlewares;
using Eventix.Modules.Events.Application;
using Eventix.Modules.Events.Infrastructure;
using Eventix.Shared.Infrastructure;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();
builder.AddSwaggerConfig();
builder.Services.AddEventsModule(builder.Configuration);

builder.Services.AddApplication([AssemblyReference.Assembly]);
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty);
builder.Configuration.AddModuleConfiguration(["events"]);
var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerConfig();

    app.ApplyMigrations();
}

EventsModule.MapEndpoints(app);

app.UseSerilogRequestLogging();

app.Run();