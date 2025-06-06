using Eventix.Modules.Events.Application.Abstractions.Clock;

namespace Eventix.Modules.Events.Infrastructure.Clock
{
    internal sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}