using Eventix.Modules.Events.Domain.Events.Enumerators;
using System.Text.Json.Serialization;

namespace Eventix.Modules.Events.Application.Events.UseCases.GetById
{
    public sealed record GetEventResponse
    {
        public GetEventResponse(Guid id, string title, string description,
                                    string? street, string? city, string? state,
                                    string? zipCode, string? number, string? additionalInfo,
                                    string? neighborhood, DateTime startsAtUtc,
                                    DateTime? endsAtUtc, EventStatusEnum status, Guid categoryId)
        {
            Id = id;
            Title = title;
            Description = description;
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
            Number = number;
            AdditionalInfo = additionalInfo;
            Neighborhood = neighborhood;
            StartsAtUtc = startsAtUtc;
            EndsAtUtc = endsAtUtc;
            Status = status;
            CategoryId = categoryId;
        }

        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Number { get; set; }
        public string? AdditionalInfo { get; set; }
        public string? Neighborhood { get; set; }
        public DateTime StartsAtUtc { get; set; }
        public DateTime? EndsAtUtc { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EventStatusEnum Status { get; private set; }
        public Guid CategoryId { get; private set; }
    }
}