using System.Threading;
using System.Threading.Tasks;

namespace CRM.Application.Common.Interfaces
{
    public interface IEventConsumer
    {
        Task ConsumeAsync(CancellationToken stoppingToken);
    }
}
