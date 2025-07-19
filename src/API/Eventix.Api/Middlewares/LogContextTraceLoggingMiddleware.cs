using Serilog.Context;
using System.Diagnostics;

namespace Eventix.Api.Middlewares
{
    internal sealed class LogContextTraceLoggingMiddleware(RequestDelegate next)
    {
        public Task Invoke(HttpContext context)
        {
            var traceId = Activity.Current?.TraceId.ToString();

            using (LogContext.PushProperty("TraceId", traceId))
            {
                return next.Invoke(context);
            }
        }
    }
}