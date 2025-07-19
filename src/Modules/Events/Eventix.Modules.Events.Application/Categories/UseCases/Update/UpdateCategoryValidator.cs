using Eventix.Modules.Events.Domain.Categories.Entities;
using Eventix.Modules.Events.Domain.Categories.Errors;
using FluentValidation;

namespace Eventix.Modules.Events.Application.Categories.UseCases.Update
{
    internal sealed class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.CategoryId)
                .NotEqual(Guid.Empty)
                .WithMessage(CategoryErrors.CategoryIdMustNotBeEmpty.Description);

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(CategoryErrors.NameMustBeNotEmpty.Description)
                .MaximumLength(Category.MAX_NAME_LENGTH)
                .WithMessage(CategoryErrors.NameMustBeLessThan100Characters.Description)
                .MinimumLength(Category.MIN_NAME_LENGTH)
                .WithMessage(CategoryErrors.CategoryMinLength.Description);
        }
    }
}