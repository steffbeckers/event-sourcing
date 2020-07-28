using CRM.Application.Common.Interfaces;
using CRM.Domain.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Persistence.EventStore
{
    public class EventsService<TA, TKey> : IEventsService<TA, TKey> where TA : class, IAggregateRoot<TKey>
    {
        private readonly IEventsRepository<TA, TKey> _eventsRepository;
        // TODO
        //private readonly IEventProducer<TA, TKey> _eventProducer;

        public EventsService(
            IEventsRepository<TA, TKey> eventsRepository
            // TODO
            //IEventProducer<TA, TKey> eventProducer
         )
        {
            _eventsRepository = eventsRepository ?? throw new ArgumentNullException(nameof(eventsRepository));
            // TODO
            //_eventProducer = eventProducer ?? throw new ArgumentNullException(nameof(eventProducer));
        }

        public async Task PersistAsync(TA aggregateRoot)
        {
            if (null == aggregateRoot)
                throw new ArgumentNullException(nameof(aggregateRoot));

            if (!aggregateRoot.Events.Any())
                return;

            await _eventsRepository.AppendAsync(aggregateRoot);
            // TODO
            //await _eventProducer.DispatchAsync(aggregateRoot);
        }

        public Task<TA> RehydrateAsync(TKey key)
        {
            return _eventsRepository.RehydrateAsync(key);
        }
    }
}
