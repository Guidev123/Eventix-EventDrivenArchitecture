using Eventix.Modules.Events.Domain.Events.Enumerators;
using Eventix.Modules.Events.Domain.Events.ValueObjects;
using System.Text.Json.Serialization;

namespace Eventix.Modules.Events.Application.Events.Get
{
    public record GetEventByIdResponse
    {
        public GetEventByIdResponse(Guid id, string title, string description, Location? location, DateTime startsAtUtc, DateTime? endsAtUtc, EventStatusEnum status)
        {
            Id = id;
            Title = title;
            Description = description;
            Location = location;
            StartsAtUtc = startsAtUtc;
            EndsAtUtc = endsAtUtc;
            Status = status;
        }

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Location? Location { get; private set; }
        public DateTime StartsAtUtc { get; private set; }
        public DateTime? EndsAtUtc { get; private set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EventStatusEnum Status { get; private set; }
    }
}