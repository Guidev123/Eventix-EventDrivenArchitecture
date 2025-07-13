using EventStore.Client;

namespace Eventix.Shared.Infrastructure.EventSourcing
{
    internal interface IEventStoreService
    {
        EventStoreClient GetStoreClientConnection();
    }
}