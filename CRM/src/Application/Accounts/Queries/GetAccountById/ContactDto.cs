using System;

namespace CRM.Application.Accounts.Queries.GetAccountById
{
    public class ContactDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsPrimary { get; set; }
        public int? SortOrder { get; set; }
    }
}