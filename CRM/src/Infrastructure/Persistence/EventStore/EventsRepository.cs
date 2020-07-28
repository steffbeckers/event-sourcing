using CRM.Application.Common.Interfaces;
using CRM.Domain.Common;
using CRM.Domain.Events;
using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Persistence.EventStore
{
    public class EventsRepository<TA, TKey> : IEventsRepository<TA, TKey>
        where TA : class, IAggregateRoot<TKey>
    {
        private readonly IEventStoreConnection _eventStoreConnection;
        private readonly IEventDeserializer _eventDeserializer;
        private readonly string _streamBaseName;

        public EventsRepository(
            IEventStoreConnection eventStoreConnection,
            IEventDeserializer eventDeserializer
        )
        {
            _eventStoreConnection = eventStoreConnection;
            _eventDeserializer = eventDeserializer;
            var aggregateType = typeof(TA);
            _streamBaseName = aggregateType.Name;
        }

        public async Task AppendAsync(TA aggregateRoot)
        {
            if (null == aggregateRoot)
                throw new ArgumentNullException(nameof(aggregateRoot));

            if (!aggregateRoot.Events.Any())
                return;

            var streamName = GetStreamName(aggregateRoot.Id);

            var firstEvent = aggregateRoot.Events.First();
            var version = firstEvent.AggregateVersion - 1;

            using var transaction = await _eventStoreConnection.StartTransactionAsync(streamName, version);

            try
            {
                foreach (var @event in aggregateRoot.Events)
                {
                    var eventData = Map(@event);
                    await transaction.WriteAsync(eventData);
                }

                await transaction.CommitAsync();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private string GetStreamName(TKey aggregateKey)
        {
            var streamName = $"{_streamBaseName}_{aggregateKey}";
            return streamName;
        }

        public async Task<TA> RehydrateAsync(TKey key)
        {
            var streamName = GetStreamName(key);

            var events = new List<IDomainEvent<TKey>>();

            StreamEventsSlice currentSlice;
            long nextSliceStart = StreamPosition.Start;
            do
            {
                currentSlice = await _eventStoreConnection.ReadStreamEventsForwardAsync(streamName, nextSliceStart, 200, false);

                nextSliceStart = currentSlice.NextEventNumber;

                events.AddRange(currentSlice.Events.Select(Map));
            } while (!currentSlice.IsEndOfStream);

            var result = BaseAggregateRoot<TA, TKey>.Create(events.OrderBy(e => e.AggregateVersion));

            return result;
        }

        private IDomainEvent<TKey> Map(ResolvedEvent resolvedEvent)
        {
            var meta = System.Text.Json.JsonSerializer.Deserialize<EventMeta>(resolvedEvent.Event.Metadata);
            return _eventDeserializer.Deserialize<TKey>(meta.EventType, resolvedEvent.Event.Data);
        }

        private static EventData Map(IDomainEvent<TKey> @event)
        {
            var json = System.Text.Json.JsonSerializer.Serialize((dynamic)@event);
            var data = Encoding.UTF8.GetBytes(json);

            var eventType = @event.GetType();
            var meta = new EventMeta()
            {
                EventType = eventType.AssemblyQualifiedName
            };
            var metaJson = System.Text.Json.JsonSerializer.Serialize(meta);
            var metadata = Encoding.UTF8.GetBytes(metaJson);

            var eventPayload = new EventData(Guid.NewGuid(), eventType.Name, true, data, metadata);
            return eventPayload;
        }

        internal struct EventMeta
        {
            public string EventType { get; set; }
        }
    }
}
