using Eventix.Api.Extensions;
using Eventix.Shared.Presentation.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

namespace Eventix.Api.Configurations
{
    public static class PipelineConfiguration
    {
        public static WebApplication UsePipeline(this WebApplication app)
        {
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

            return app;
        }
    }
}