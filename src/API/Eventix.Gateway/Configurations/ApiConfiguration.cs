using Eventix.Gateway.Authentication;
using Eventix.Gateway.Middlewares;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

namespace Eventix.Gateway.Configurations
{
    public static class ApiConfiguration
    {
        private const string SERVICE_NAME = "Eventix.Gateway";

        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, loggerConfig)
                => loggerConfig.ReadFrom.Configuration(context.Configuration));

            builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

            builder.Services
                .AddOpenTelemetry()
                .ConfigureResource(c => c.AddService(SERVICE_NAME))
                .WithTracing(tracing =>
                {
                    tracing
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddSource("Yarp.ReverseProxy");

                    tracing.AddOtlpExporter();
                });

            builder.Services.AddAuthentication().AddJwtBearer();
            builder.Services.AddAuthorization();
            builder.Services.ConfigureOptions<JwtBearerConfigureOptions>();

            return builder;
        }

        public static WebApplication UsePipelineConfig(this WebApplication app, WebApplicationBuilder builder)
        {
            app.UseSerilogRequestLogging();
            app.UseLogContextTraceLogging();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapReverseProxy();

            return app;
        }
    }
}