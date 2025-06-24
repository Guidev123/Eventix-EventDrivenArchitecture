using Eventix.Shared.Domain.DomainObjects;

namespace Eventix.Shared.Domain.Interfaces
{
    public interface IRepository<TAggregateRoot> : IDisposable where TAggregateRoot : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}