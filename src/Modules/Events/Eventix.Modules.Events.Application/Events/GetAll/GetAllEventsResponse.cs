using Eventix.Modules.Events.Domain.Events.Enumerators;
using System.Text.Json.Serialization;

namespace Eventix.Modules.Events.Application.Events.GetAll
{
    public record GetAllEventsResponse
    {
        public GetAllEventsResponse(Guid id, string title, string description, DateTime startsAtUtc, DateTime? endsAtUtc, EventStatusEnum status)
        {
            Id = id;
            Title = title;
            Description = description;
            StartsAtUtc = startsAtUtc;
            EndsAtUtc = endsAtUtc;
            Status = status;
        }

        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime StartsAtUtc { get; set; }
        public DateTime? EndsAtUtc { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EventStatusEnum Status { get; private set; }
    }
}