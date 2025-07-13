using Eventix.Api.Configurations;
using Eventix.Shared.Application.EventSourcing;

WebApplicationBuilder builder = WebApplication
    .CreateBuilder(args)
    .ConfigureServices();

var app = builder
    .Build()
    .UsePipeline(builder);

app.MapGet("api/v1/event-sourcing/{id:guid}", async (Guid id, IEventSourcingRepository eve) =>
{
    var result = await eve.GetAllAsync(id);
    return result.Any() ? Results.Ok(result) : Results.BadRequest();
});

app.Run();

public partial class Program;