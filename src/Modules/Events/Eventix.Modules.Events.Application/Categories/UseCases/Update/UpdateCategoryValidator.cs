using Eventix.Modules.Events.Domain.Categories.Entities;
using Eventix.Modules.Events.Domain.Categories.Errors;
using FluentValidation;

namespace Eventix.Modules.Events.Application.Categories.UseCases.Update
{
    public sealed class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(CategoryErrors.NameMustBeNotEmpty.Description)
                .MaximumLength(Category.MAX_NAME_LENGTH)
                .WithMessage(CategoryErrors.NameMustBeLessThan100Characters.Description);
        }
    }
}