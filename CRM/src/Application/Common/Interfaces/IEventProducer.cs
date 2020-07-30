using CRM.Domain.Common;
using System.Threading.Tasks;

namespace CRM.Application.Common.Interfaces
{
    public interface IEventProducer<in TA, in TKey>
        where TA : IAggregateRoot<TKey>
    {
        Task DispatchAsync(TA aggregateRoot);
    }
}
