using CRM.Application.Common.Interfaces;
using CRM.Domain.Common;
using CRM.Domain.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Kafka
{
    public class EventConsumerFactory : IEventConsumerFactory
    {
        private readonly IServiceScopeFactory scopeFactory;

        public EventConsumerFactory(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public IEventConsumer Build<TA, TKey>() where TA : IAggregateRoot<TKey>
        {
            using var scope = scopeFactory.CreateScope();
            EventConsumer<TA, TKey> consumer = scope.ServiceProvider.GetRequiredService<EventConsumer<TA, TKey>>();

            async Task onEventReceived(object s, IDomainEvent<TKey> e)
            {
                var @event = EventReceivedFactory.Create((dynamic)e);

                using var innerScope = scopeFactory.CreateScope();

                IMediator mediator = innerScope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Publish(@event, CancellationToken.None);
            }
            consumer.EventReceived += onEventReceived;

            return consumer;
        }
    }
}
