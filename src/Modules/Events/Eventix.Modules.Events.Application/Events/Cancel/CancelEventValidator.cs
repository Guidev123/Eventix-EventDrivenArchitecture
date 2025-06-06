using FluentValidation;

namespace Eventix.Modules.Events.Application.Events.Cancel
{
    public sealed class CancelEventValidator : AbstractValidator<CancelEventCommand>
    {
        public CancelEventValidator()
        {
        }
    }
}