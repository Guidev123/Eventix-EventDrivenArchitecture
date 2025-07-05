using Eventix.Shared.Domain.DomainObjects;
using Eventix.Shared.Domain.ValueObjects.Errors;

namespace Eventix.Shared.Domain.ValueObjects
{
    public sealed record DateRange : ValueObject
    {
        public const int MINIMUM_START_TIME_IN_HOURS = 1;

        public DateRange(DateTime startsAtUtc, DateTime? endsAtUtc)
        {
            StartsAtUtc = startsAtUtc;
            EndsAtUtc = endsAtUtc;
            Validate();
        }

        private DateRange()
        { }

        public DateTime StartsAtUtc { get; }
        public DateTime? EndsAtUtc { get; }
        private bool IsInValidRange
            => StartsAtUtc > DateTime.UtcNow;

        public static implicit operator DateRange((DateTime start, DateTime? end) range)
            => new(range.start, range.end);
        protected override void Validate()
        {
            AssertionConcern.EnsureTrue(IsInValidRange, ValueObjectErrors.StartDateMustBeInFuture.Description);

            if (EndsAtUtc.HasValue)
            {
                AssertionConcern.EnsureTrue(
                    EndsAtUtc.Value > StartsAtUtc,
                    ValueObjectErrors.EndDateMustBeAfterStartDate.Description);
            }
        }
    }
}