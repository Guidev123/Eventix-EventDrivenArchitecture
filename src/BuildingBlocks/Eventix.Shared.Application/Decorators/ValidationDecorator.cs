using Eventix.Shared.Application.Messaging;
using Eventix.Shared.Domain.Responses;
using FluentValidation;
using FluentValidation.Results;
using MidR.Interfaces;
using System.Reflection;

namespace Eventix.Shared.Application.Decorators
{
    public static class ValidationDecorator
    {
        public sealed class RequestHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> innerHandler,
            IEnumerable<IValidator<TRequest>> validators)
            : IRequestHandler<TRequest, TResponse>
            where TRequest : IRequest<TResponse>, IBaseCommand
            where TResponse : Result
        {
            public async Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default)
            {
                var validationFailures = await ValidateAsync(request, cancellationToken);

                if (validationFailures.Length == 0)
                    return await innerHandler.ExecuteAsync(request, cancellationToken);

                if (typeof(TResponse).IsGenericType &&
                    typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
                {
                    Type resultType = typeof(TResponse).GetGenericArguments()[0];

                    MethodInfo? failureMethod = typeof(Result<>)
                                    .MakeGenericType(resultType)
                                    .GetMethod(nameof(Result<object>.ValidationFailure));

                    if (failureMethod is not null)
                    {
                        var validationError = CreateValidationError(validationFailures);
                        var result = failureMethod.Invoke(null, [validationError]);
                        if (result is not null) return (TResponse)result;
                    }
                }
                else if (typeof(TResponse) == typeof(Result))
                {
                    return (TResponse)(object)Result.Failure(CreateValidationError(validationFailures));
                }

                throw new ValidationException(validationFailures);
            }

            private async Task<ValidationFailure[]> ValidateAsync(TRequest request, CancellationToken cancellationToken = default)
            {
                if (!validators.Any()) return Array.Empty<ValidationFailure>();

                var context = new ValidationContext<TRequest>(request);

                ValidationResult[] validationResults = await Task.WhenAll(
                    validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

                ValidationFailure[] validationFailures = validationResults
                    .Where(validationResult => !validationResult.IsValid)
                    .SelectMany(validationResult => validationResult.Errors)
                    .ToArray();

                return validationFailures;
            }

            private static ValidationError CreateValidationError(ValidationFailure[] validationFailures)
                => new(validationFailures.Select(x => Error.Problem(x.ErrorCode, x.ErrorMessage)).ToArray());
        }
    }
}