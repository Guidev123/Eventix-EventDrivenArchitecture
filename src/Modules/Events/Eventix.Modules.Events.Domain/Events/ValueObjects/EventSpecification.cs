namespace Eventix.Modules.Events.Domain.Events.ValueObjects
{
    public sealed record EventSpecification
    {
        public EventSpecification(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public string Title { get; private set; }
        public string Description { get; private set; }

        public static implicit operator EventSpecification((string title, string description) spec)
            => new(spec.title, spec.description);
    }
}