namespace Eventix.Modules.Events.Domain.Evemts.ValueObjects
{
    public sealed record Location
    {
        public Location(string street, string number, string additionalInfo, string neighborhood, string zipCode, string city, string state)
        {
            Street = street;
            Number = number;
            AdditionalInfo = additionalInfo;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            City = city;
            State = state;
        }

        public string Street { get; private set; } = string.Empty;
        public string Number { get; private set; } = string.Empty;
        public string AdditionalInfo { get; private set; } = string.Empty;
        public string Neighborhood { get; private set; } = string.Empty;
        public string ZipCode { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string State { get; private set; } = string.Empty;

        public static implicit operator Location((string street, string number, string additionalInfo, string neighborhood, string zipCode, string city, string state) location)
            => new(location.street, location.number, location.additionalInfo, location.neighborhood, location.zipCode, location.city, location.state);
    }
}