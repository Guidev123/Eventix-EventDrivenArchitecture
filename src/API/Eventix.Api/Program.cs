using Eventix.Api.Configurations;
using Eventix.Api.Extensions;
using Eventix.Modules.Events.Application;
using Eventix.Modules.Events.Infrastructure;
using Eventix.Shared.Application;
using Eventix.Shared.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.AddSwaggerConfig();

builder.Services.AddEventsModule(builder.Configuration);

builder.Services.AddApplication([AssemblyReference.Assembly]);
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerConfig();

    app.ApplyMigrations();
}

EventsModule.MapEndpoints(app);

app.Run();