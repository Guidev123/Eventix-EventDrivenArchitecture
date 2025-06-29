using Eventix.Api.Configurations;

WebApplicationBuilder builder = WebApplication
    .CreateBuilder(args)
    .ConfigureServices();

var app = builder
    .Build()
    .UsePipeline(builder);

app.Run();

public partial class Program
{ }