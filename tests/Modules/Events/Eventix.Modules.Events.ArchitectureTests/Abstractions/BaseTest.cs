using Eventix.Modules.Events.Domain.Events.Entities;
using Eventix.Modules.Events.Infrastructure;
using System.Reflection;
using PresentationModule = Eventix.Modules.Events.Presentation.PresentationModule;

namespace Eventix.Modules.Events.ArchitectureTests.Abstractions;

public abstract class BaseTest
{
    protected static readonly Assembly ApplicationAssembly = typeof(Events.Application.AssemblyReference).Assembly;

    protected static readonly Assembly DomainAssembly = typeof(Event).Assembly;

    protected static readonly Assembly InfrastructureAssembly = typeof(EventsModule).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(PresentationModule).Assembly;
}