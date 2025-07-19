using Eventix.Shared.Domain.Responses;

namespace Eventix.Modules.Events.Domain.Categories.Errors
{
    public static class CategoryErrors
    {
        public static Error NotFound(Guid categoryId) =>
            Error.NotFound("Categories.NotFound", $"The category with the identifier {categoryId} was not found");

        public static readonly Error AlreadyArchived = Error.Problem(
            "Categories.AlreadyArchived",
            "The category was already archived");

        public static readonly Error CategoryMinLength = Error.Problem(
            "Categories.CategoryMinLength",
            "Category name must be more than 2 of length");

        public static readonly Error FailToCreate = Error.Problem(
            "Categories.Create",
            "An error occurred while creating the category");

        public static readonly Error FailToArchive = Error.Problem(
            "Categories.Archive",
            "An error occurred while archiving the category");

        public static readonly Error NameMustBeNotEmpty = Error.Problem(
            "Categories.NameMustBeNotEmpty",
            "The category name must not be empty");

        public static readonly Error NameMustBeLessThan100Characters = Error.Problem(
            "Categories.NameMustBeLessThan100Characters",
            "The category name must be less than 100 characters");

        public static readonly Error FailToUpdate = Error.Problem(
            "Categories.Update",
            "An error occurred while updating the category");

        public static readonly Error CategoryIdMustNotBeEmpty = Error.Problem(
              "Categories.CategoryIdMustNotBeEmpty",
              "The category ID must not be empty");
    }
}