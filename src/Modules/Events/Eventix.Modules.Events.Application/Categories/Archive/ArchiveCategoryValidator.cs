using FluentValidation;

namespace Eventix.Modules.Events.Application.Categories.Archive
{
    public sealed class ArchiveCategoryValidator : AbstractValidator<ArchiveCategoryCommand>
    {
        public ArchiveCategoryValidator()
        {
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category ID must not be empty.");
        }
    }
}