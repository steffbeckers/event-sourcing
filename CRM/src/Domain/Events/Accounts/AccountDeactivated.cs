using CRM.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Domain.Events.Accounts
{
    public class AccountDeactivated : BaseDomainEvent<Account, Guid>
    {
        /// <summary>
        /// for deserialization
        /// </summary>
        private AccountDeactivated() { }

        public AccountDeactivated(Account account, string userId) : base(account)
        {
            Name = account.Name;
            UserId = userId;
        }

        public string Name { get; private set; }
        public string UserId { get; private set; }
    }
}
