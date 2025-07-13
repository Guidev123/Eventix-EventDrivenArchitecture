using Eventix.Shared.Application.EventSourcing;
using Eventix.Shared.Domain.DomainEvents;
using Eventix.Shared.Infrastructure.Extensions;
using EventStore.Client;
using Newtonsoft.Json;
using System.Text;

namespace Eventix.Shared.Infrastructure.EventSourcing
{
    internal sealed class EventSourcingRepository(IEventStoreService eventStoreService) : IEventSourcingRepository
    {
        public async Task<IEnumerable<StoredEvent>> GetAllAsync(Guid aggregateId)
        {
            var events = eventStoreService.GetStoreClientConnection()
                .ReadStreamAsync(
                    Direction.Forwards,
                    aggregateId.ToString(),
                    StreamPosition.Start,
                    500,
                    resolveLinkTos: false
                );

            List<StoredEvent> resolvedEvents = [];
            await foreach (var resolvedEvent in events)
            {
                var dataEncoded = Encoding.UTF8.GetString(resolvedEvent.Event.Data.Span);
                var jsonData = JsonConvert.DeserializeObject<IDomainEvent>(dataEncoded, SerializerExtension.Instance)
                    ?? throw new ArgumentNullException("Fail to Deserialize Domain Event");

                var storedEvent = new StoredEvent(
                    resolvedEvent.Event.EventId.ToGuid(),
                    resolvedEvent.Event.EventType,
                    jsonData.OccurredOnUtc, dataEncoded);

                resolvedEvents.Add(storedEvent);
            }

            return resolvedEvents.OrderBy(x => x.OccuredAt);
        }

        public async Task SaveAsync<TEvent>(TEvent @event) where TEvent : IDomainEvent =>
            await eventStoreService.GetStoreClientConnection().AppendToStreamAsync(@event.AggregateId.ToString(),
                StreamState.Any, CreateEventData(@event));

        private static IEnumerable<EventData> CreateEventData<TEvent>(TEvent @event) where TEvent : IDomainEvent
        {
            yield return new(
                Uuid.NewUuid(),
                @event.GetType().Name,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event, SerializerExtension.Instance)),
                null,
                "application/json"
                );
        }
    }
}