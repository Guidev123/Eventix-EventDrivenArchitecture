namespace Eventix.Shared.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync(CancellationToken cancellationToken = default);
    }
}