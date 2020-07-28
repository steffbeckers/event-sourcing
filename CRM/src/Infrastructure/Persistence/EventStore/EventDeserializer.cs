using CRM.Application.Common.Interfaces;
using CRM.Domain.Events;
using CRM.Infrastructure.Persistence.EventStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CRM.Infrastructure.Persistence.EventStore
{
    public class EventDeserializer : IEventDeserializer
    {
        private readonly IEnumerable<Assembly> _assemblies;

        public EventDeserializer(IEnumerable<Assembly> assemblies)
        {
            _assemblies = assemblies ?? new[] {Assembly.GetExecutingAssembly()};
        }

        public IDomainEvent<TKey> Deserialize<TKey>(string type, byte[] data)
        {
            var jsonData = System.Text.Encoding.UTF8.GetString(data);
            return this.Deserialize<TKey>(type, jsonData);
        }

        public IDomainEvent<TKey> Deserialize<TKey>(string type, string data)
        {
            // TODO: Cache types
            var eventType = _assemblies.Select(a => a.GetType(type, false))
                                .FirstOrDefault(t => t != null) ?? Type.GetType(type);
            if (null == eventType)
                throw new ArgumentOutOfRangeException(nameof(type), $"invalid event type: {type}");

            // As of 01/10/2020, "Deserialization to reference types without a parameterless constructor isn't supported."
            // https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to
            // apparently it's being worked on: https://github.com/dotnet/runtime/issues/29895
            
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject(data, eventType,
                new Newtonsoft.Json.JsonSerializerSettings()
                {
                    ConstructorHandling = Newtonsoft.Json.ConstructorHandling.AllowNonPublicDefaultConstructor,
                    ContractResolver = new PrivateSetterContractResolver()
                });

            return (IDomainEvent<TKey>) result;
        }
    }
}