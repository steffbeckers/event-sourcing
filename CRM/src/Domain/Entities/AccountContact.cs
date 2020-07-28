using CRM.Domain.Common;
using System;

namespace CRM.Domain.Entities
{
    public class AccountContact
    {
        public Guid AccountId { get; set; }
        public Account Account { get; set; }

        public Guid ContactId { get; set; }
        public Contact Contact { get; set; }

        public bool IsPrimary { get; set; }
        public int? SortOrder { get; set; }

        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
