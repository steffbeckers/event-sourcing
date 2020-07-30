using CRM.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Infrastructure.Kafka
{
    public static class EventReceivedFactory
    {
        public static EventReceived<TE> Create<TE>(TE @event)
        {
            return new EventReceived<TE>(@event);
        }
    }
}
