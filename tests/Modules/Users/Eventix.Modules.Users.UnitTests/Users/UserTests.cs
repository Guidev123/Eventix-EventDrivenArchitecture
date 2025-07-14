using Eventix.Modules.Users.Domain.Users.DomainEvents;
using Eventix.Modules.Users.Domain.Users.Entities;
using Eventix.Modules.Users.Domain.Users.Models;
using Eventix.Modules.Users.UnitTests.Abstractions;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Eventix.Modules.Users.UnitTests.Users;

public class UserTests : BaseTest
{
    [Fact(DisplayName = "Create Should Return User")]
    [Trait("Users Unit Tests", "User Tests")]
    public void Create_ShouldReturnUser()
    {
        // Act
        var user = User.Create(
            Guid.NewGuid(),
            _faker.Internet.Email(),
            _faker.Name.FirstName(),
            _faker.Name.LastName());

        // Assert
        user.Should().NotBeNull();
    }

    [Fact(DisplayName = "Create Should Return User With Member Role")]
    [Trait("Users Unit Tests", "User Tests")]
    public void Create_ShouldReturnUser_WithMemberRole()
    {
        // Act
        var user = User.Create(
            Guid.NewGuid(),
            _faker.Internet.Email(),
            _faker.Name.FirstName(),
            _faker.Name.LastName());

        // Assert
        user.Roles.Single().Should().Be(Role.Member);
    }

    [Fact(DisplayName = "Create Should Raise Domain Event When User Created")]
    [Trait("Users Unit Tests", "User Tests")]
    public void Create_ShouldRaiseDomainEvent_WhenUserCreated()
    {
        // Act
        var user = User.Create(
            Guid.NewGuid(),
            _faker.Internet.Email(),
            _faker.Name.FirstName(),
            _faker.Name.LastName());

        // Assert
        var domainEvent =
            AssertDomainEventWasPublished<UserCreatedDomainEvent>(user);

        domainEvent.UserId.Should().Be(user.Id);
    }

    [Fact(DisplayName = "Update Should Raise Domain Event When User Updated")]
    [Trait("Users Unit Tests", "User Tests")]
    public void Update_ShouldRaiseDomainEvent_WhenUserUpdated()
    {
        // Arrange
        var user = User.Create(
            Guid.NewGuid(),
            _faker.Internet.Email(),
            _faker.Name.FirstName(),
            _faker.Name.LastName());

        // Act
        user.UpdateName(user.Name.LastName, user.Name.FirstName);

        // Assert
        var domainEvent =
            AssertDomainEventWasPublished<UserNameUpdatedDomainEvent>(user);

        domainEvent.UserId.Should().Be(user.Id);
        domainEvent.FirstName.Should().Be(user.Name.FirstName);
        domainEvent.LastName.Should().Be(user.Name.LastName);
    }

    [Fact(DisplayName = "Update Should Not Raise Domain Event When User Not Updated")]
    [Trait("Users Unit Tests", "User Tests")]
    public void Update_ShouldNotRaiseDomainEvent_WhenUserNotUpdated()
    {
        // Arrange
        var user = User.Create(
            Guid.NewGuid(),
            _faker.Internet.Email(),
            _faker.Name.FirstName(),
            _faker.Name.LastName());

        user.ClearDomainEvents();

        // Act
        user.UpdateName(user.Name.FirstName, user.Name.LastName);

        // Assert
        user.DomainEvents.Should().BeEmpty();
    }
}