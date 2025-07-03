using Eventix.Modules.Attendance.Domain.Events.Errors;
using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Attendance.Domain.Events.ValueObjects
{
    public sealed record EventSpecification : ValueObject
    {
        public const int TITLE_MIN_LENGTH = 3;
        public const int TITLE_MAX_LENGTH = 100;

        public const int DESCRIPTION_MIN_LENGTH = 10;
        public const int DESCRIPTION_MAX_LENGTH = 1000;
        public EventSpecification(string title, string description)
        {
            Title = title;
            Description = description;
            Validate();
        }

        public string Title { get; private set; }
        public string Description { get; private set; }

        public static implicit operator EventSpecification((string title, string description) spec)
            => new(spec.title, spec.description);
        protected override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(Title, EventErrors.TitleIsRequired.Description);
            AssertionConcern.EnsureLengthInRange(Title, TITLE_MIN_LENGTH, TITLE_MAX_LENGTH, EventErrors.TitleLengthInvalid.Description);
            AssertionConcern.EnsureNotEmpty(Description, EventErrors.DescriptionIsRequired.Description);
            AssertionConcern.EnsureLengthInRange(Description, DESCRIPTION_MIN_LENGTH, DESCRIPTION_MAX_LENGTH, EventErrors.DescriptionLengthInvalid.Description);
        }
    }
}