using Bogus;
using Eventix.Shared.Domain.DomainEvents;
using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Modules.Ticketing.UnitTests.Abstractions;

public abstract class BaseTest
{
    protected static readonly Faker _faker = new();

    public static T AssertDomainEventWasPublished<T>(Entity entity)
        where T : IDomainEvent => entity.DomainEvents.OfType<T>().SingleOrDefault()
        ?? throw new Exception($"{typeof(T).Name} was not published");
}