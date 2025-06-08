using Eventix.Modules.Events.Domain.Events.Errors;
using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Events.Domain.Events.ValueObjects
{
    public sealed record EventSpecification : ValueObject
    {
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
            AssertionConcern.EnsureLengthInRange(Title, 3, 100, EventErrors.TitleLengthInvalid.Description);
            AssertionConcern.EnsureNotEmpty(Description, EventErrors.DescriptionIsRequired.Description);
            AssertionConcern.EnsureLengthInRange(Description, 10, 1000, EventErrors.DescriptionLengthInvalid.Description);
        }
    }
}