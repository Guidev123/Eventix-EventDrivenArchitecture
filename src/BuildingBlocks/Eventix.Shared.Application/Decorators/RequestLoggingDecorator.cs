using Eventix.Shared.Application.Exceptions;
using Eventix.Shared.Domain.Responses;
using Microsoft.Extensions.Logging;
using MidR.Interfaces;
using Serilog.Context;
using System.Diagnostics;

namespace Eventix.Shared.Application.Decorators
{
    public static class RequestLoggingDecorator
    {
        public sealed class RequestHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> innerHandler,
            ILogger<IRequestHandler<TRequest, TResponse>> logger)
            : IRequestHandler<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
            where TResponse : Result
        {
            public async Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken)
            {
                var requestName = typeof(TRequest).Name;
                var requestModule = GetRequestModule(typeof(TRequest).FullName!);

                Activity.Current?.SetTag("request.module", requestModule);
                Activity.Current?.SetTag("request.name", requestName);

                var stopwatch = Stopwatch.StartNew();
                using (LogContext.PushProperty("Module", requestModule))
                {
                    try
                    {
                        logger.LogInformation("Processing request: {RequestName}", requestName);

                        var result = await innerHandler.ExecuteAsync(request, cancellationToken);

                        stopwatch.Stop();
                        var executionTime = stopwatch.ElapsedMilliseconds;

                        if (result.IsSuccess)
                        {
                            logger.LogInformation("Request: {RequestName} processed successfully in {ExecutionTimeInMilliseconds}ms",
                                requestName, executionTime);
                        }
                        else
                        {
                            using (LogContext.PushProperty("Error", result.Error, true))
                            {
                                logger.LogError("Request: {RequestName} failed in {ExecutionTimeInMilliseconds}ms",
                                    requestName, executionTime);
                            }
                        }

                        return result;
                    }
                    catch (Exception ex)
                    {
                        stopwatch.Stop();
                        var executionTime = stopwatch.ElapsedMilliseconds;

                        logger.LogError(ex, "Request: {RequestName} failed in {ExecutionTimeInMilliseconds}ms with unhandled exception", requestName, executionTime);

                        throw new EventixException(typeof(TRequest).Name, innerException: ex);
                    }
                }
            }

            private static string GetRequestModule(string requestName) => requestName.Split('.')[2];
        }
    }
}