using Eventix.Modules.Users.ArchitectureTests.Abstractions;
using NetArchTest.Rules;

namespace Eventix.Modules.Users.ArchitectureTests.Layers;

public class LayerTests : BaseTest
{
    [Fact(DisplayName = "Domain Layer Should Not Have Dependency On Application Layer")]
    [Trait("Users Architecture Tests", "Layers Test")]
    public void DomainLayer_ShouldNotHaveDependencyOn_ApplicationLayer()
    {
        Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(ApplicationAssembly.GetName().Name)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact(DisplayName = "Domain Layer Should Not Have Dependency On Infrastructure Layer")]
    [Trait("Users Architecture Tests", "Layers Test")]
    public void DomainLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        Types
            .InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact(DisplayName = "Application Layer Should Not Have Dependency On Infrastructure Layer")]
    [Trait("Users Architecture Tests", "Layers Test")]
    public void ApplicationLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact(DisplayName = "Application Layer Should Not Have Dependency On Presentation Layer")]
    [Trait("Users Architecture Tests", "Layers Test")]
    public void ApplicationLayer_ShouldNotHaveDependencyOn_PresentationLayer()
    {
        Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact(DisplayName = "Presentation Layer Should Not Have Dependency On Infrastructure Layer")]
    [Trait("Users Architecture Tests", "Layers Test")]
    public void PresentationLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        Types.InAssembly(PresentationAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult()
            .ShouldBeSuccessful();
    }
}