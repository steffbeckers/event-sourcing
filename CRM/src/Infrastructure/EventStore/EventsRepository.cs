using CRM.Application.Common.Interfaces;
using CRM.Domain.Common;
using CRM.Domain.Events;
using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CRM.Infrastructure.EventStore
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
            return $"{_streamBaseName}_{aggregateKey}";
        }

        public async Task<TA> RehydrateAsync(TKey key)
        {
            string streamName = GetStreamName(key);

            List<IDomainEvent<TKey>> events = new List<IDomainEvent<TKey>>();

            StreamEventsSlice currentSlice;
            long nextSliceStart = StreamPosition.Start;
            do
            {
                currentSlice = await _eventStoreConnection.ReadStreamEventsForwardAsync(streamName, nextSliceStart, 200, false);

                nextSliceStart = currentSlice.NextEventNumber;

                events.AddRange(currentSlice.Events.Select(Map));
            } while (!currentSlice.IsEndOfStream);

            return BaseAggregateRoot<TA, TKey>.Create(events.OrderBy(e => e.AggregateVersion));
        }

        private IDomainEvent<TKey> Map(ResolvedEvent resolvedEvent)
        {
            EventMeta meta = JsonSerializer.Deserialize<EventMeta>(resolvedEvent.Event.Metadata);

            return _eventDeserializer.Deserialize<TKey>(meta.EventType, resolvedEvent.Event.Data);
        }

        private static EventData Map(IDomainEvent<TKey> @event)
        {
            string json = JsonSerializer.Serialize((dynamic)@event);
            byte[] data = Encoding.UTF8.GetBytes(json);

            Type eventType = @event.GetType();
            EventMeta meta = new EventMeta()
            {
                EventType = eventType.AssemblyQualifiedName
            };
            string metaJson = JsonSerializer.Serialize(meta);
            byte[] metadata = Encoding.UTF8.GetBytes(metaJson);

            return new EventData(Guid.NewGuid(), eventType.Name, true, data, metadata);
        }

        internal struct EventMeta
        {
            public string EventType { get; set; }
        }
    }
}
