using Eventix.Modules.Ticketing.Domain.Events.Errors;
using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Ticketing.Domain.Events.ValueObjects
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
            AssertionConcern.EnsureNotEmpty(Street, EventErrors.StreetIsRequired.Description);
            AssertionConcern.EnsureNotEmpty(Number, EventErrors.NumberIsRequired.Description);
            AssertionConcern.EnsureNotEmpty(Neighborhood, EventErrors.NeighborhoodIsRequired.Description);
            AssertionConcern.EnsureNotEmpty(ZipCode, EventErrors.ZipCodeIsRequired.Description);
            AssertionConcern.EnsureNotEmpty(City, EventErrors.CityIsRequired.Description);
            AssertionConcern.EnsureNotEmpty(State, EventErrors.StateIsRequired.Description);
        }
    }
}