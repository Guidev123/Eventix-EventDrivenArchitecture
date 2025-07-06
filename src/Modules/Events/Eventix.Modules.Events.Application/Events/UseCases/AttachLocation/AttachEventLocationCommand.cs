using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Events.Application.Events.UseCases.AttachLocation
{
    public sealed record AttachEventLocationCommand : ICommand
    {
        public AttachEventLocationCommand(string street, string number, string additionalInfo, string neighborhood, string zipCode, string city, string state)
        {
            Street = street;
            Number = number;
            AdditionalInfo = additionalInfo;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            City = city;
            State = state;
        }

        public Guid EventId { get; private set; }
        public string Street { get; init; }
        public string Number { get; init; }
        public string AdditionalInfo { get; init; }
        public string Neighborhood { get; init; }
        public string ZipCode { get; init; }
        public string City { get; init; }
        public string State { get; init; }
        public void SetEventId(Guid eventId)
        {
            EventId = eventId;
        }
    }
}