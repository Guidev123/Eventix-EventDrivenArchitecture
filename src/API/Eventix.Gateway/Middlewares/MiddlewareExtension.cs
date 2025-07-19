namespace Eventix.Gateway.Middlewares
{
    internal static class MiddlewareExtension
    {
        internal static IApplicationBuilder UseLogContextTraceLogging(this IApplicationBuilder app)
        {
            app.UseMiddleware<LogContextTraceLoggingMiddleware>();

            return app;
        }
    }
}