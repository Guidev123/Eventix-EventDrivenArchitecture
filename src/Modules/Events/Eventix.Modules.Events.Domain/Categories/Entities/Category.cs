using Eventix.Modules.Events.Domain.Categories.DomainEvents;
using Eventix.Modules.Events.Domain.Categories.Errors;
using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Events.Domain.Categories.Entities
{
    public sealed class Category : Entity, IAggregateRoot
    {
        public const int MAX_NAME_LENGTH = 100;
        public const int MIN_NAME_LENGTH = 2;

        private Category(string name)
        {
            Name = name;
            IsArchived = false;
            Validate();
        }

        private Category()
        { }

        public string Name { get; private set; } = string.Empty;

        public bool IsArchived { get; private set; }

        public static Category Create(string name)
        {
            var category = new Category(name);

            category.Raise(new CategoryCreatedDomainEvent(category.Id, category.Name));

            return category;
        }

        public void Archive()
        {
            if (IsArchived) return;

            IsArchived = true;

            Raise(new CategoryArchivedDomainEvent(Id));
        }

        public void Rename(string name)
        {
            if (Name == name) return;

            Name = name;

            Raise(new CategoryRenamedDomainEvent(Id, Name));
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(Name, CategoryErrors.NameMustBeNotEmpty.Description);
            AssertionConcern.EnsureTrue(Name.Length <= MAX_NAME_LENGTH, CategoryErrors.NameMustBeNotEmpty.Description);
        }
    }
}