using Eventix.Modules.Ticketing.ArchitectureTests.Abstractions;
using Eventix.Shared.Domain.DomainEvents;
using Eventix.Shared.Domain.DomainObjects;
using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;

namespace Eventix.Modules.Ticketing.ArchitectureTests.Domain;

public class DomainTests : BaseTest
{
    [Fact(DisplayName = "Domain Events Should Have Name Ending With DomainEvent")]
    [Trait("Ticketing Architecture Tests", "Domain Tests")]
    public void DomainEvent_ShouldHaveNameEndingWith_DomainEvent()
    {
        // Arrange & Act & Assert
        Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(DomainEvent))
            .Should()
            .HaveNameEndingWith("DomainEvent")
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact(DisplayName = "Entities Should Have Private Parameterless Constructor")]
    [Trait("Ticketing Architecture Tests", "Domain Tests")]
    public void Entities_ShouldHave_PrivateParameterlessConstructor()
    {
        // Arrange & Act
        IEnumerable<Type> entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();

        var failingTypes = new List<Type>();
        foreach (Type entityType in entityTypes)
        {
            ConstructorInfo[] constructors = entityType.GetConstructors(BindingFlags.NonPublic |
                                                                        BindingFlags.Instance);

            if (!constructors.Any(c => c.IsPrivate && c.GetParameters().Length == 0))
            {
                failingTypes.Add(entityType);
            }
        }

        // Assert
        failingTypes.Should().BeEmpty();
    }

    [Fact(DisplayName = "Entities Should Only Have Private Constructors")]
    [Trait("Ticketing Architecture Tests", "Domain Tests")]
    public void Entities_ShouldOnlyHave_PrivateConstructors()
    {
        // Arrange & Act
        IEnumerable<Type> entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();

        var failingTypes = new List<Type>();
        foreach (Type entityType in entityTypes)
        {
            ConstructorInfo[] constructors = entityType.GetConstructors(BindingFlags.Public |
                                                                        BindingFlags.Instance);

            if (constructors.Any())
            {
                failingTypes.Add(entityType);
            }
        }

        // Assert
        failingTypes.Should().BeEmpty();
    }
}