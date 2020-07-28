using CRM.Application.Common.Mappings;
using CRM.Domain.Entities;
using System;

namespace CRM.Application.Accounts.Queries.GetAccounts
{
    public class AccountDto : IMapFrom<Account>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
