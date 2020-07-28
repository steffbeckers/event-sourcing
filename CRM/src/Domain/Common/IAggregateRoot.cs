using CRM.Domain.Events;
using System.Collections.Generic;

namespace CRM.Domain.Common
{
    public interface IAggregateRoot<out TKey> : IEntity<TKey>
    {
        public long Version { get; }
        IReadOnlyCollection<IDomainEvent<TKey>> Events { get; }
        void ClearEvents();
    }
}