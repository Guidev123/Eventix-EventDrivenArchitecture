namespace Eventix.Modules.Events.Domain.Shared
{
    public interface IDomainEvent
    {
        Guid Id { get; }
        DateTime OccurredOnUtc { get; }
    }
}