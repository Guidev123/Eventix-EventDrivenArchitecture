using Eventix.Api.Configurations;
using Eventix.Shared.Application.EventSourcing;

WebApplicationBuilder builder = WebApplication
    .CreateBuilder(args)
    .ConfigureServices();

var app = builder
    .Build()
    .UsePipeline(builder);

app.Run();

public partial class Program;