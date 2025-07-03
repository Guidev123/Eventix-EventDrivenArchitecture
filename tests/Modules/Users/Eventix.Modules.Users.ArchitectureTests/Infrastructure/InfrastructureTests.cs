using Eventix.Modules.Users.ArchitectureTests.Abstractions;
using Eventix.Shared.Application.EventBus;
using MassTransit;
using NetArchTest.Rules;

namespace Eventix.Modules.Users.ArchitectureTests.Infrastructure;

public class InfrastructureTests : BaseTest
{
    [Fact(DisplayName = "Integration Event Handler Should Have Name Ending With Consumer")]
    [Trait("Users Arhcitecture Tests", "Infrastructure Tests")]
    public void IntegrationEventHandler_Should_NotBePublic()
    {
        Types.InAssembly(InfrastructureAssembly)
            .That()
            .ImplementInterface(typeof(IIntegrationEventHandler<>))
            .Or()
            .Inherit(typeof(IntegrationEventHandler<>))
            .Should()
            .NotBePublic()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact(DisplayName = "Integration Event Handler Should Be Sealed")]
    [Trait("Users Arhcitecture Tests", "Infrastructure Tests")]
    public void IntegrationEventHandler_Should_BeSealed()
    {
        Types.InAssembly(InfrastructureAssembly)
            .That()
            .ImplementInterface(typeof(IIntegrationEventHandler<>))
            .Or()
            .Inherit(typeof(IntegrationEventHandler<>))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact(DisplayName = "Integration Event Handler Should Have Name Ending With IntegrationEventHandler")]
    [Trait("Users Arhcitecture Tests", "Infrastructure Tests")]
    public void IntegrationEventHandler_ShouldHave_NameEndingWith_IntegrationEventConsumer()
    {
        Types.InAssembly(InfrastructureAssembly)
            .That()
            .ImplementInterface(typeof(IIntegrationEventHandler<>))
            .Or()
            .Inherit(typeof(IntegrationEventHandler<>))
            .Should()
            .HaveNameMatching(".*IntegrationEventHandler(\\`\\d+)?$")
            .GetResult()
            .ShouldBeSuccessful();
    }
}