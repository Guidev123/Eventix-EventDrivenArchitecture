namespace Eventix.Shared.Application.EventSourcing
{
    public sealed record StoredEvent(
    Guid Id, string Type,
    DateTime OccuredAt, string Data
    );
}