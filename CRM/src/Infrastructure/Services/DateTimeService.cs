using CRM.Application.Common.Interfaces;
using System;

namespace CRM.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
