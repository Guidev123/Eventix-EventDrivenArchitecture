using Eventix.Shared.Domain.DomainObjects;
using Eventix.Shared.Domain.ValueObjects.Errors;

namespace Eventix.Shared.Domain.ValueObjects
{
    public sealed record Location : ValueObject
    {
        public const int STREET_MIN_LENGTH = 2;
        public const int STREET_MAX_LENGTH = 100;
        public const int NUMBER_MAX_LENGTH = 20;
        public const int NEIGHBORHOOD_MAX_LENGTH = 50;
        public const int ZIPCODE_MAX_LENGTH = 10;
        public const int CITY_MAX_LENGTH = 50;
        public const int STATE_LENGTH = 2;
        public const int STATE_MAX_LENGTH = 2;

        public Location(string street, string number, string additionalInfo, string neighborhood, string zipCode, string city, string state)
        {
            Street = street;
            Number = number;
            AdditionalInfo = additionalInfo;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            City = city;
            State = state;
            Validate();
        }

        private Location()
        { }

        public string Street { get; private set; } = string.Empty;
        public string Number { get; private set; } = string.Empty;
        public string AdditionalInfo { get; private set; } = string.Empty;
        public string Neighborhood { get; private set; } = string.Empty;
        public string ZipCode { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string State { get; private set; } = string.Empty;

        public static implicit operator Location((string street, string number, string additionalInfo, string neighborhood, string zipCode, string city, string state) location)
            => new(location.street, location.number, location.additionalInfo, location.neighborhood, location.zipCode, location.city, location.state);
        protected override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(Street, ValueObjectErrors.StreetIsRequired.Description);
            AssertionConcern.EnsureNotEmpty(Number, ValueObjectErrors.NumberIsRequired.Description);
            AssertionConcern.EnsureNotEmpty(Neighborhood, ValueObjectErrors.NeighborhoodIsRequired.Description);
            AssertionConcern.EnsureNotEmpty(ZipCode, ValueObjectErrors.ZipCodeIsRequired.Description);
            AssertionConcern.EnsureNotEmpty(City, ValueObjectErrors.CityIsRequired.Description);
            AssertionConcern.EnsureNotEmpty(State, ValueObjectErrors.StateIsRequired.Description);
        }
    }
}