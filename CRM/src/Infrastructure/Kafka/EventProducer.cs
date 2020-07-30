using Confluent.Kafka;
using CRM.Application.Common.Interfaces;
using CRM.Domain.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Kafka
{
    public class EventProducer<TA, TKey> : IDisposable, IEventProducer<TA, TKey>
        where TA : IAggregateRoot<TKey>
    {
        private IProducer<TKey, string> _producer;
        private readonly string _topicName;

        public EventProducer(IConfiguration configuration)
        {
            string topicPrefix = configuration.GetSection("Kafka")
                .GetValue<string>("TopicPrefix");
            Type aggregateType = typeof(TA);

            _topicName = $"{topicPrefix}-{aggregateType.Name}";

            ProducerConfig producerConfig = new ProducerConfig()
            {
                BootstrapServers = configuration.GetSection("Kafka")
                    .GetValue<string>("BootstrapServers")
            };

            _producer = new ProducerBuilder<TKey, string>(producerConfig)
                .SetKeySerializer(new KeySerializer<TKey>())
                .Build();
        }

        public async Task DispatchAsync(TA aggregateRoot)
        {
            if (null == aggregateRoot)
                throw new ArgumentNullException(nameof(aggregateRoot));

            if (!aggregateRoot.Events.Any())
                return;

            foreach (var @event in aggregateRoot.Events)
            {
                var eventType = @event.GetType();

                var serialized = JsonSerializer.Serialize(@event, eventType);

                var headers = new Headers
                {
                    {"aggregate", Encoding.UTF8.GetBytes(@event.AggregateId.ToString())},
                    {"type", Encoding.UTF8.GetBytes(eventType.AssemblyQualifiedName)}
                };

                var message = new Message<TKey, string>()
                {
                    Key = @event.AggregateId,
                    Value = serialized,
                    Headers = headers
                };

                await _producer.ProduceAsync(_topicName, message);
            }

            aggregateRoot.ClearEvents();
        }

        public void Dispose()
        {
            _producer?.Dispose();
            _producer = null;
        }
    }
}
