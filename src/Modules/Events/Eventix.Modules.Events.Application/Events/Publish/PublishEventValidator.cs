using FluentValidation;

namespace Eventix.Modules.Events.Application.Events.Publish
{
    public sealed class PublishEventValidator : AbstractValidator<PublishEventCommand>
    {
        public PublishEventValidator()
        {
        }
    }
}