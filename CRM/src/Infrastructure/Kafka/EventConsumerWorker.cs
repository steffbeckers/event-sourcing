using CRM.Application.Common.Interfaces;
using CRM.Domain.Aggregates;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Kafka
{
    public class EventsConsumerWorker : BackgroundService
    {
        private readonly IEventConsumerFactory _eventConsumerFactory;

        public EventsConsumerWorker(IEventConsumerFactory eventConsumerFactory)
        {
            _eventConsumerFactory = eventConsumerFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IEnumerable<IEventConsumer> consumers = new[]
            {
                _eventConsumerFactory.Build<Account, Guid>()
            };

            var tc = Task.WhenAll(consumers.Select(c => c.ConsumeAsync(stoppingToken)));
            await tc;
        }
    }
}
