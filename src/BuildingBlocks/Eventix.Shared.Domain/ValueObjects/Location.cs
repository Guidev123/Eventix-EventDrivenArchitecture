using Eventix.Shared.Domain.DomainObjects;
using Eventix.Shared.Domain.ValueObjects.Errors;

namespace Eventix.Shared.Domain.ValueObjects
{
    public sealed record Location : ValueObject
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