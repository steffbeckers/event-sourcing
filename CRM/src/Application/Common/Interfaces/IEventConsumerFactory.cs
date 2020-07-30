using CRM.Domain.Common;

namespace CRM.Application.Common.Interfaces
{
    public interface IEventConsumerFactory
    {
        IEventConsumer Build<TA, TKey>() where TA : IAggregateRoot<TKey>;
    }
}
