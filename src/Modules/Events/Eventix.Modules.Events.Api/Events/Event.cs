namespace Eventix.Modules.Events.Api.Events
{
    public sealed class Event
    {
        public Event(Guid id, string title, string description, string location, DateTime startsAtUtc, DateTime? endsAtUtc)
        {
            Id = id;
            Title = title;
            Description = description;
            Location = location;
            StartsAtUtc = startsAtUtc;
            EndsAtUtc = endsAtUtc;
            Status = EventStatusEnum.Draft;
        }

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Location { get; private set; }
        public DateTime StartsAtUtc { get; private set; }
        public DateTime? EndsAtUtc { get; private set; }
        public EventStatusEnum Status { get; private set; }
    }
}