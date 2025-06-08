using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Events.Domain.Events.ValueObjects
{
    public sealed record DateRange : ValueObject
    {
        public DateRange(DateTime startsAtUtc, DateTime? endsAtUtc)
        {
            StartsAtUtc = startsAtUtc;
            EndsAtUtc = endsAtUtc;
            Validate();
        }

        public DateTime StartsAtUtc { get; }
        public DateTime? EndsAtUtc { get; }
        private bool IsInValidRange
            => StartsAtUtc > DateTime.UtcNow;

        public static implicit operator DateRange((DateTime start, DateTime? end) range)
            => new(range.start, range.end);
        protected override void Validate()
        {
            AssertionConcern.EnsureTrue(IsInValidRange, EventErrors.StartDateMustBeInFuture.Description);

            if (EndsAtUtc.HasValue)
            {
                AssertionConcern.EnsureTrue(
                    EndsAtUtc.Value > StartsAtUtc,
                    EventErrors.EndDateMustBeAfterStartDate.Description);
            }
        }
    }
}