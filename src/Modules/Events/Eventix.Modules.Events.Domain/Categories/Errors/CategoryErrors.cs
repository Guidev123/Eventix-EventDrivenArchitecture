using Eventix.Modules.Events.Domain.Shared;

namespace Eventix.Modules.Events.Domain.Categories.Errors
{
    public static class CategoryErrors
    {
        public static Error NotFound(Guid categoryId) =>
            Error.NotFound("Categories.NotFound", $"The category with the identifier {categoryId} was not found");

        public static readonly Error AlreadyArchived = Error.Problem(
            "Categories.AlreadyArchived",
            "The category was already archived");

        public static readonly Error FailToCreate = Error.Problem(
            "Categories.Create",
            "An error occurred while creating the category");

        public static readonly Error FailToArchive = Error.Problem(
            "Categories.Archive",
            "An error occurred while archiving the category");
    }
}