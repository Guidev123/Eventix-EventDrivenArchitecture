using Eventix.Modules.Events.Domain.Categories.DomainEvents;
using Eventix.Modules.Events.Domain.Categories.Entities;
using Eventix.Modules.Events.UnitTests.Abstractions;
using Eventix.Shared.Domain.Responses;
using FluentAssertions;

namespace Eventix.Modules.Events.UnitTests.Categories
{
    public class CategoryTests : BaseTest
    {
        [Fact(DisplayName = "Create Should Raise Domain Event When Category Is Created")]
        [Trait("Events Unit Tests", "Category Tests")]
        public void Create_ShouldRaiseDomainEvent_WhenCategoryIsCreated()
        {
            // Act
            Result<Category> result = Category.Create(_faker.Music.Genre());

            // Assert
            var domainEvent = AssertDomainEventWasPublished<CategoryCreatedDomainEvent>(result.Value);

            domainEvent.CategoryId.Should().Be(result.Value.Id);
        }

        [Fact(DisplayName = "Archive Should Raise Domain Event When Category Is Archived")]
        [Trait("Events Unit Tests", "Category Tests")]
        public void Archive_ShouldRaiseDomainEvent_WhenCategoryIsArchived()
        {
            // Arrange
            Result<Category> result = Category.Create(_faker.Music.Genre());
            var category = result.Value;

            // Act
            category.Archive();

            // Assert
            var domainEvent = AssertDomainEventWasPublished<CategoryArchivedDomainEvent>(category);
            domainEvent.CategoryId.Should().Be(category.Id);
        }

        [Fact(DisplayName = "Change Name Should Raise Domain Event When Category Name Is Changed")]
        [Trait("Events Unit Tests", "Category Tests")]
        public void ChangeName_ShouldRaiseDomainEvent_WhenCategoryNameIsChanged()
        {
            // Arrange
            Result<Category> result = Category.Create(_faker.Music.Genre());
            var category = result.Value;
            category.ClearDomainEvents();

            var newName = _faker.Music.Genre();

            // Act
            category.Rename(newName);

            // Assert
            var domainEvent = AssertDomainEventWasPublished<CategoryRenamedDomainEvent>(category);
            domainEvent.CategoryId.Should().Be(category.Id);
        }
    }
}