using CRM.Domain.Common;
using System.Threading.Tasks;

namespace CRM.Application.Common.Interfaces
{
    public interface IEventsService<TA, TKey>
        where TA : class, IAggregateRoot<TKey>
    {
        Task PersistAsync(TA aggregateRoot);
        Task<TA> RehydrateAsync(TKey key);
    }
}
