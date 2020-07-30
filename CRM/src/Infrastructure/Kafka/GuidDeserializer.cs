using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Infrastructure.Kafka
{
    internal class GuidDeserializer : IDeserializer<Guid>
    {
        public Guid Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return new Guid(data);
        }
    }
}
