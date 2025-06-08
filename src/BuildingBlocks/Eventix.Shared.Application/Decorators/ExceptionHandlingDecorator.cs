using Eventix.Shared.Application.Exceptions;
using Eventix.Shared.Domain.Responses;
using Microsoft.Extensions.Logging;
using MidR.Interfaces;

namespace Eventix.Shared.Application.Decorators
{
    public static class ExceptionHandlingDecorator
    {
        public sealed class RequestHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> innerHandler,
            ILogger<IRequestHandler<TRequest, TResponse>> logger)
            : IRequestHandler<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
            where TResponse : Result
        {
            public async Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    return await innerHandler.ExecuteAsync(request, cancellationToken);
                }
                catch (Exception exception)
                {
                    logger.LogError(exception, "Request: {RequestName} failed with unhandled exception.", typeof(TRequest).Name);

                    throw new EventixException(typeof(TRequest).Name, innerException: exception);
                }
            }
        }
    }
}