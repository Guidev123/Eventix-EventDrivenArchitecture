using MidR.Interfaces;

namespace Eventix.Shared.Domain.DomainObjects
{
    public interface IEvent : INotification
    {
        Guid CorrelationId { get; }
        DateTime OccurredOnUtc { get; }
    }
}