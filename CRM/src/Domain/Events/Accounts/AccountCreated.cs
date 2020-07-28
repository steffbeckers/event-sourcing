using CRM.Domain.Entities;
using System;

namespace CRM.Domain.Events.Accounts
{
    public class AccountCreated : BaseDomainEvent<Account, Guid>
    {
        /// <summary>
        /// for deserialization
        /// </summary>
        private AccountCreated() { }

        public AccountCreated(Account account) : base(account)
        {
            Id = account.Id;
            Name = account.Name;
            Website = account.Website;
            Email = account.Email;
            PhoneNumber = account.PhoneNumber;
            IsActive = account.IsActive;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Website { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public bool IsActive { get; private set; }
    }
}