using Eventix.Modules.Users.ArchitectureTests.Abstractions;
using MassTransit;
using NetArchTest.Rules;

namespace Eventix.Modules.Users.ArchitectureTests.Infrastructure;

public class InfrastructureTests : BaseTest
{
    [Fact(DisplayName = "Integration Event Handler Should Be Sealed")]
    [Trait("Users Arhcitecture Tests", "Infrastructure Tests")]
    public void IntegrationEventHandler_Should_BeSealed()
    {
        // Arrange & Act & Assert
        Types.InAssembly(PresentationAssembly)
            .That()
            .ImplementInterface(typeof(IConsumer<>))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact(DisplayName = "Integration Event Handler Should Have Name Ending With Consumer")]
    [Trait("Users Arhcitecture Tests", "Infrastructure Tests")]
    public void IntegrationEventHandler_ShouldHave_NameEndingWith_Consumer()
    {
        // Arrange & Act & Assert
        Types.InAssembly(PresentationAssembly)
            .That()
            .ImplementInterface(typeof(IConsumer<>))
            .Should()
            .HaveNameEndingWith("Consumer")
            .GetResult()
            .ShouldBeSuccessful();
    }
}