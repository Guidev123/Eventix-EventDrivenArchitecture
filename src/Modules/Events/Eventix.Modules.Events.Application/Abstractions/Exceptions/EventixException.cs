using Eventix.Modules.Events.Domain.Shared;

namespace Eventix.Modules.Events.Application.Abstractions.Exceptions
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