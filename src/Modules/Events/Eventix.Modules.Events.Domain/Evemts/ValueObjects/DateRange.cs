namespace Eventix.Modules.Events.Domain.Evemts.ValueObjects
{
    public sealed record DateRange
    {
        public DateRange(DateTime startsAtUtc, DateTime? endsAtUtc)
        {
            StartsAtUtc = startsAtUtc;
            EndsAtUtc = endsAtUtc;
        }

        public DateTime StartsAtUtc { get; }
        public DateTime? EndsAtUtc { get; }

        public static implicit operator DateRange((DateTime start, DateTime? end) range)
            => new(range.start, range.end);
    }
}