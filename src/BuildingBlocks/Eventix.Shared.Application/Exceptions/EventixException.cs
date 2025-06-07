using Eventix.Shared.Domain.Responses;

namespace Eventix.Shared.Application.Exceptions
{
    public sealed class EventixException : Exception
    {
        public EventixException(string requestName, Error? error = default, Exception? innerException = default)
            : base("Application exception", innerException)
        {
            RequestName = requestName;
            Error = error;
        }

        public string RequestName { get; }

        public Error? Error { get; }
    }
}