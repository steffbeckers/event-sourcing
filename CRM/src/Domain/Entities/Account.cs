using CRM.Domain.Common;
using CRM.Domain.Events;
using CRM.Domain.Events.Accounts;
using System;
using System.Collections.Generic;

namespace CRM.Domain.Entities
{
    public class Account : BaseAggregateRoot<Account, Guid>
    {
        public Account()
        {}

        public Account(
            Guid id,
            string name,
            string website,
            string email,
            string phoneNumber,
            bool isActive
        ) : base(id)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentOutOfRangeException(nameof(name));

            Name = name;
            Website = website;
            Email = email;
            PhoneNumber = phoneNumber;
            IsActive = isActive;

            this.AddEvent(new AccountCreated(this));
        }

        public string Name { get; private set; }
        public string Website { get; private set; }
        // TODO: Use value object instead of string?
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public bool IsActive { get; private set; }

        public ICollection<AccountContact> AccountContact { get; set; }

        public string CreatedBy { get; private set; }
        public DateTime Created { get; private set; }
        public string LastModifiedBy { get; private set; }
        public DateTime? LastModified { get; private set; }

        protected override void Apply(IDomainEvent<Guid> @event)
        {
            switch (@event)
            {
                case AccountCreated account:
                    Id = account.AggregateId;
                    Name = account.Name;
                    Website = account.Website;
                    Email = account.Email;
                    PhoneNumber = account.PhoneNumber;
                    IsActive = account.IsActive;
                    break;
            }
        }

        public static Account Create(
            string name,
            string website,
            string email,
            string phoneNumber,
            bool isActive
        )
        {
            return new Account(
                Guid.NewGuid(),
                name,
                website,
                email,
                phoneNumber,
                isActive
            );
        }
    }
}
