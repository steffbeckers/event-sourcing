using CRM.Domain.Events;

namespace CRM.Application.Common.Interfaces
{
    public interface IEventDeserializer
    {
        IDomainEvent<TKey> Deserialize<TKey>(string type, byte[] data);
        IDomainEvent<TKey> Deserialize<TKey>(string type, string data);
    }
}
