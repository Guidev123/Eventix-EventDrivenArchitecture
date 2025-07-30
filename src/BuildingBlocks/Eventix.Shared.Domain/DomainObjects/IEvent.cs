namespace Eventix.Shared.Domain.DomainObjects
{
    public interface IEvent
    {
        Guid CorrelationId { get; }
        DateTime OccurredOnUtc { get; }
    }
}