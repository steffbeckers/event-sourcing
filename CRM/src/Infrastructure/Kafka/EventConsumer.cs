using Confluent.Kafka;
using CRM.Application.Common.Interfaces;
using CRM.Domain.Common;
using CRM.Domain.Events;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Kafka
{
    public class EventConsumer<TA, TKey> : IDisposable, IEventConsumer where TA : IAggregateRoot<TKey>
    {
        private IConsumer<TKey, string> _consumer;
        private readonly IEventDeserializer _eventDeserializer;

        public EventConsumer(IConfiguration configuration, IEventDeserializer eventDeserializer)
        {
            _eventDeserializer = eventDeserializer;

            string topicPrefix = configuration.GetSection("Kafka")
                .GetValue<string>("TopicPrefix");
            Type aggregateType = typeof(TA);

            string topicName = $"{topicPrefix}-{aggregateType.Name}";

            ConsumerConfig consumerConfig = new ConsumerConfig
            {
                GroupId = configuration.GetSection("Kafka")
                    .GetValue<string>("ConsumerGroupId"),
                BootstrapServers = configuration.GetSection("Kafka")
                    .GetValue<string>("BootstrapServers"),
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnablePartitionEof = true
            };

            _consumer = new ConsumerBuilder<TKey, string>(consumerConfig)
                .SetKeyDeserializer(new KeyDeserializerFactory().Create<TKey>())
                .Build();

            _consumer.Subscribe(topicName);
        }

        public Task ConsumeAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () =>
            {
                var topics = string.Join(",", _consumer.Subscription);

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        ConsumeResult<TKey, string> cr = _consumer.Consume(stoppingToken);
                        if (cr.IsPartitionEOF)
                            continue;

                        IHeader messageTypeHeader = cr.Message.Headers.First(h => h.Key == "type");
                        string eventType = Encoding.UTF8.GetString(messageTypeHeader.GetValueBytes());

                        IDomainEvent<TKey> @event = _eventDeserializer.Deserialize<TKey>(eventType, cr.Message.Value);
                        if (@event == null)
                            throw new SerializationException($"unable to deserialize event {eventType} : {cr.Message.Value}");

                        await OnEventReceived(@event);
                    }
                    catch (OperationCanceledException ex)
                    {
                        OnConsumerStopped(ex);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        OnExceptionThrown(ex);
                    }
                }
            }, stoppingToken);
        }

        public delegate Task EventReceivedHandler(object sender, IDomainEvent<TKey> e);
        public event EventReceivedHandler EventReceived;
        protected virtual Task OnEventReceived(IDomainEvent<TKey> e)
        {
            var handler = EventReceived;
            return handler?.Invoke(this, e);
        }

        public delegate void ExceptionThrownHandler(object sender, Exception e);
        public event ExceptionThrownHandler ExceptionThrown;
        protected virtual void OnExceptionThrown(Exception e)
        {
            var handler = ExceptionThrown;
            handler?.Invoke(this, e);
        }

        public delegate void ConsumerStoppedHandler(object sender, Exception e);
        public event ConsumerStoppedHandler ConsumerStopped;
        protected virtual void OnConsumerStopped(Exception e)
        {
            var handler = ConsumerStopped;
            handler?.Invoke(this, e);
        }

        public void Dispose()
        {
            _consumer?.Dispose();
            _consumer = null;
        }
    }
}
