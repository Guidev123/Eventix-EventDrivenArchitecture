using Eventix.Gateway.Configurations;

var builder = WebApplication
    .CreateBuilder(args)
    .ConfigureServices();

var app = builder
    .Build()
    .UsePipelineConfig(builder);

app.Run();