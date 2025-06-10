using MidR.Interfaces;

namespace Eventix.Shared.Domain.DomainEvents
{
    public interface IDomainEvent : INotification
    {
        Guid Id { get; }
        DateTime OccurredOnUtc { get; }
    }
}