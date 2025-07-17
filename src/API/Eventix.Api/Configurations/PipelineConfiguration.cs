using Eventix.Api.Extensions;
using Eventix.Shared.Presentation.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

namespace Eventix.Api.Configurations
{
    public static class PipelineConfiguration
    {
        public static WebApplication UsePipeline(this WebApplication app, WebApplicationBuilder builder)
        {
            app.UseExceptionHandler();

            if (app.Environment.IsDevelopment()
                || app.Environment.IsEnvironment("Testing"))
            {
                app.MapOpenApi();
                app.UseSwaggerConfig(builder);

                app.ApplyMigrations();
            }

            app.MapEndpoints();

            if (!app.Environment.IsEnvironment("Testing"))
            {
                app.MapHealthChecks("healthz", new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                app.UseSerilogRequestLogging();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}