using Eventix.Shared.Domain.Responses;
using Microsoft.Extensions.Logging;
using MidR.Interfaces;

namespace Eventix.Shared.Application.Behaviors
{
    public static class LoggingDecorator
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

                logger.LogInformation("Processing request: {RequestName}", requestName);

                var result = await innerHandler.ExecuteAsync(request, cancellationToken);

                if (result.IsSuccess)
                {
                    logger.LogInformation("Request: {RequestName} processed successfully.", requestName);
                }
                else
                {
                    logger.LogError("Request: {RequestName} failed with error: {Error}", requestName, result.Error);
                }

                return result;
            }
        }
    }
}