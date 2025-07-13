using EventStore.Client;
using Microsoft.Extensions.Configuration;

namespace Eventix.Shared.Infrastructure.EventSourcing
{
    internal class EventStoreService(IConfiguration configuration) : IEventStoreService
    {
        public EventStoreClient GetStoreClientConnection()
        {
            var connectionString = configuration.GetConnectionString("EventStore") ?? string.Empty;
            return new EventStoreClient(EventStoreClientSettings.Create(connectionString));
        }
    }
}