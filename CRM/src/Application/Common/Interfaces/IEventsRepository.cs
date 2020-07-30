using CRM.Domain.Common;
using System.Threading.Tasks;

namespace CRM.Application.Common.Interfaces
{
    public interface IEventsRepository<TA, TKey>
        where TA : class, IAggregateRoot<TKey>
    {
        Task AppendAsync(TA aggregateRoot);
        Task<TA> RehydrateAsync(TKey key);
    }
}
