using Eventix.Shared.Domain.ValueObjects;
using Eventix.Shared.Domain.ValueObjects.Errors;
using FluentValidation;

namespace Eventix.Modules.Events.Application.Events.UseCases.AttachLocation
{
    internal sealed class AttachEventLocationValidator : AbstractValidator<AttachEventLocationCommand>
    {
        public AttachEventLocationValidator()
        {
            RuleFor(c => c.Street)
                .NotEmpty().WithMessage(ValueObjectErrors.StreetIsRequired.Description)
                .MinimumLength(Location.STREET_MIN_LENGTH).WithMessage(ValueObjectErrors.StreetTooShort(Location.STREET_MIN_LENGTH).Description)
                .MaximumLength(Location.STREET_MAX_LENGTH).WithMessage(ValueObjectErrors.StreetTooLong(Location.STREET_MAX_LENGTH).Description);

            RuleFor(c => c.Number)
                .NotEmpty().WithMessage(ValueObjectErrors.NumberIsRequired.Description)
                .MaximumLength(Location.NUMBER_MAX_LENGTH).WithMessage(ValueObjectErrors.NumberTooLong(Location.NUMBER_MAX_LENGTH).Description);

            RuleFor(c => c.Neighborhood)
                .NotEmpty().WithMessage(ValueObjectErrors.NeighborhoodIsRequired.Description)
                .MaximumLength(Location.NEIGHBORHOOD_MAX_LENGTH).WithMessage(ValueObjectErrors.NeighborhoodTooLong(Location.NEIGHBORHOOD_MAX_LENGTH).Description);

            RuleFor(c => c.ZipCode)
                .NotEmpty().WithMessage(ValueObjectErrors.ZipCodeIsRequired.Description)
                .MaximumLength(Location.ZIPCODE_MAX_LENGTH).WithMessage(ValueObjectErrors.ZipCodeTooLong(Location.ZIPCODE_MAX_LENGTH).Description);

            RuleFor(c => c.City)
                .NotEmpty().WithMessage(ValueObjectErrors.CityIsRequired.Description)
                .MaximumLength(Location.CITY_MAX_LENGTH).WithMessage(ValueObjectErrors.CityTooLong(Location.CITY_MAX_LENGTH).Description);

            RuleFor(c => c.State)
                .NotEmpty().WithMessage(ValueObjectErrors.StateIsRequired.Description)
                .Length(Location.STATE_MAX_LENGTH).WithMessage(ValueObjectErrors.StateInvalidLength(Location.STATE_MAX_LENGTH).Description);
        }
    }
}