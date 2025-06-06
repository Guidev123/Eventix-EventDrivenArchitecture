using Eventix.Modules.Events.Domain.Shared;
using Microsoft.AspNetCore.Http;

namespace Eventix.Modules.Events.Presentation.Extensions
{
    public static class ApiResults
    {
        public static IResult Problem(Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException();
            }

            return Results.Problem(
                title: GetTitle(result.Error),
                detail: GetDetail(result.Error),
                type: GetType(result.Error.Type),
                statusCode: GetStatusCode(result.Error.Type),
                extensions: GetErrors(result));

            static string GetTitle(Error error) =>
                error.Type switch
                {
                    ErrorTypeEnum.Validation => error.Code,
                    ErrorTypeEnum.Problem => error.Code,
                    ErrorTypeEnum.NotFound => error.Code,
                    ErrorTypeEnum.Conflict => error.Code,
                    _ => "Server failure"
                };

            static string GetDetail(Error error) =>
                error.Type switch
                {
                    ErrorTypeEnum.Validation => error.Description,
                    ErrorTypeEnum.Problem => error.Description,
                    ErrorTypeEnum.NotFound => error.Description,
                    ErrorTypeEnum.Conflict => error.Description,
                    _ => "An unexpected error occurred"
                };

            static string GetType(ErrorTypeEnum errorType) =>
                errorType switch
                {
                    ErrorTypeEnum.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    ErrorTypeEnum.Problem => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    ErrorTypeEnum.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    ErrorTypeEnum.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                    _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
                };

            static int GetStatusCode(ErrorTypeEnum errorType) =>
                errorType switch
                {
                    ErrorTypeEnum.Validation => StatusCodes.Status400BadRequest,
                    ErrorTypeEnum.Problem => StatusCodes.Status400BadRequest,
                    ErrorTypeEnum.NotFound => StatusCodes.Status404NotFound,
                    ErrorTypeEnum.Conflict => StatusCodes.Status409Conflict,
                    _ => StatusCodes.Status500InternalServerError
                };

            static Dictionary<string, object?>? GetErrors(Result result)
            {
                if (result.Error is not ValidationError validationError)
                {
                    return null;
                }

                return new Dictionary<string, object?>
                {
                    { "errors", validationError.Errors }
                };
            }
        }
    }
}