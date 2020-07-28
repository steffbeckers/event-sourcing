using CRM.Domain.Common;
using System;
using System.Collections.Generic;

namespace CRM.Domain.Entities
{
    public class Contact
    {
        public Contact()
        {
            AccountContact = new List<AccountContact>();
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }

        public ICollection<AccountContact> AccountContact { get; set; }

        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
