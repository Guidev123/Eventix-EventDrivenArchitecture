using Eventix.Modules.Events.Domain.Categories.Entities;
using FluentValidation;

namespace Eventix.Modules.Events.Application.Categories.Create
{
    public sealed class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(Category.MAX_NAME_LENGTH).WithMessage($"Name must not exceed {Category.MAX_NAME_LENGTH} characters.");
        }
    }
}