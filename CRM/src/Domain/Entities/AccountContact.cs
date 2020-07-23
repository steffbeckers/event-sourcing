using CRM.Domain.Common;
using System;

namespace CRM.Domain.Entities
{
    public class AccountContact : AuditableEntity
    {
        public Guid AccountId { get; set; }
        public Account Account { get; set; }

        public Guid ContactId { get; set; }
        public Contact Contact { get; set; }

        public bool IsPrimary { get; set; }
        public int? SortOrder { get; set; }
    }
}
