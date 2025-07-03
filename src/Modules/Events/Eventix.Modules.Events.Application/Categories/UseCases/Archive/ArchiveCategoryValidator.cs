using Eventix.Modules.Events.Domain.Categories.Errors;
using FluentValidation;

namespace Eventix.Modules.Events.Application.Categories.UseCases.Archive
{
    internal sealed class ArchiveCategoryValidator : AbstractValidator<ArchiveCategoryCommand>
    {
        public ArchiveCategoryValidator()
        {
            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage(CategoryErrors.CategoryIdMustNotBeEmpty.Description);
        }
    }
}