using Eventix.Api.Configurations;
using Eventix.Api.Extensions;
using Eventix.Modules.Events.Api;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.AddSwaggerConfig();
builder.Services.AddEventsModule(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerConfig();

    app.ApplyMigrations();
}

EventsModule.MapEndpoints(app);

app.Run();