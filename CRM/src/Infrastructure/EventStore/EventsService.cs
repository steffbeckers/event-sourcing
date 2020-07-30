using CRM.Application.Common.Interfaces;
using CRM.Domain.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Infrastructure.EventStore
{
    public class EventsService<TA, TKey> : IEventsService<TA, TKey> where TA : class, IAggregateRoot<TKey>
    {
        private readonly IEventsRepository<TA, TKey> _eventsRepository;
        private readonly IEventProducer<TA, TKey> _eventProducer;

        public EventsService(
            IEventsRepository<TA, TKey> eventsRepository,
            IEventProducer<TA, TKey> eventProducer
         )
        {
            _eventsRepository = eventsRepository ?? throw new ArgumentNullException(nameof(eventsRepository));
            _eventProducer = eventProducer ?? throw new ArgumentNullException(nameof(eventProducer));
        }

        public async Task PersistAsync(TA aggregateRoot)
        {
            if (null == aggregateRoot)
                throw new ArgumentNullException(nameof(aggregateRoot));

            if (!aggregateRoot.Events.Any())
                return;

            // Append the aggregate to the eventstore db
            await _eventsRepository.AppendAsync(aggregateRoot);

            // Dispatch the aggregate events to the event bus (Kafka)
            await _eventProducer.DispatchAsync(aggregateRoot);
        }

        public Task<TA> RehydrateAsync(TKey key)
        {
            return _eventsRepository.RehydrateAsync(key);
        }
    }
}
