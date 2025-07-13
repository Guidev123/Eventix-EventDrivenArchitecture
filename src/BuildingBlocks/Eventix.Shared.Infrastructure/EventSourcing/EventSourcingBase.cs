namespace Eventix.Shared.Infrastructure.EventSourcing
{
    public sealed record EventSourcingBase
    {
        public DateTime Timestamp { get; set; }
    }
}