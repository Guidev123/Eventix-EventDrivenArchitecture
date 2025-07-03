using Eventix.Modules.Attendance.ArchitectureTests.Abstractions;
using Eventix.Shared.Application.Messaging;
using FluentValidation;
using NetArchTest.Rules;

namespace Eventix.Modules.Attendance.ArchitectureTests.Application;

public class ApplicationTests : BaseTest
{
    [Fact(DisplayName = "Command Handlers Should Be Sealed")]
    [Trait("Attendance Architecture Tests", "Application Tests")]
    public void CommandHandler_Should_BeSealed()
    {
        // Arrange & Act & Assert
        Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Or()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact(DisplayName = "Command Handlers Should Have Name Ending With Handler")]
    [Trait("Attendance Architecture Tests", "Application Tests")]
    public void CommandHandler_ShouldHave_NameEndingWith_Handler()
    {
        // Arrange & Act & Assert
        Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Or()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Should()
            .HaveNameEndingWith("Handler")
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact(DisplayName = "Queries Should Be Sealed")]
    [Trait("Attendance Architecture Tests", "Application Tests")]
    public void Query_Should_BeSealed()
    {
        // Arrange & Act & Assert
        Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IQuery<>))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact(DisplayName = "Queries Should Have Name Ending With Query")]
    [Trait("Attendance Architecture Tests", "Application Tests")]
    public void Query_ShouldHave_NameEndingWith_Query()
    {
        // Arrange & Act & Assert
        Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IQuery<>))
            .Should()
            .HaveNameEndingWith("Query")
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact(DisplayName = "Query Handlers Should Not Be Public")]
    [Trait("Attendance Architecture Tests", "Application Tests")]
    public void QueryHandler_Should_NotBePublic()
    {
        Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .NotBePublic()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact(DisplayName = "Query Handlers Should Be Sealed")]
    [Trait("Attendance Architecture Tests", "Application Tests")]
    public void QueryHandler_Should_BeSealed()
    {
        // Arrange & Act & Assert
        Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact(DisplayName = "Query Handlers Should Have Name Ending With Handler")]
    [Trait("Attendance Architecture Tests", "Application Tests")]
    public void QueryHandler_ShouldHave_NameEndingWith_Handler()
    {
        // Arrange & Act & Assert
        Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .HaveNameEndingWith("Handler")
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact(DisplayName = "Validators Should Not Be Public")]
    [Trait("Attendance Architecture Tests", "Application Tests")]
    public void Validator_Should_NotBePublic()
    {
        // Arrange & Act & Assert
        Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(AbstractValidator<>))
            .Should()
            .NotBePublic()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact(DisplayName = "Validators Should Be Sealed")]
    [Trait("Attendance Architecture Tests", "Application Tests")]
    public void Validator_Should_BeSealed()
    {
        // Arrange & Act & Assert
        Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(AbstractValidator<>))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact(DisplayName = "Validators Should Have Name Ending With Validator")]
    [Trait("Attendance Architecture Tests", "Application Tests")]
    public void Validator_ShouldHave_NameEndingWith_Validator()
    {
        // Arrange & Act & Assert
        Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(AbstractValidator<>))
            .Should()
            .HaveNameEndingWith("Validator")
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact(DisplayName = "Domain Event Handlers Should Not Be Public")]
    [Trait("Attendance Architecture Tests", "Application Tests")]
    public void DomainEventHandler_Should_NotBePublic()
    {
        // Arrange & Act & Assert
        Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEventHandler<>))
            .Or()
            .Inherit(typeof(DomainEventHandler<>))
            .Should()
            .NotBePublic()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact(DisplayName = "Domain Event Handlers Should Be Sealed")]
    [Trait("Attendance Architecture Tests", "Application Tests")]
    public void DomainEventHandler_Should_BeSealed()
    {
        // Arrange & Act & Assert
        Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEventHandler<>))
            .Or()
            .Inherit(typeof(DomainEventHandler<>))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact(DisplayName = "Domain Event Handlers Should Have Name Ending With DomainEventHandler")]
    [Trait("Attendance Architecture Tests", "Application Tests")]
    public void DomainEventHandler_ShouldHave_NameEndingWith_DomainEventHandler()
    {
        // Arrange & Act & Assert
        Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEventHandler<>))
            .Or()
            .Inherit(typeof(DomainEventHandler<>))
            .Should()
            .HaveNameEndingWith("DomainEventHandler")
            .GetResult()
            .ShouldBeSuccessful();
    }
}