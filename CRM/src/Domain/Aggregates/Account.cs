using CRM.Domain.Common;
using CRM.Domain.Events;
using CRM.Domain.Events.Accounts;
using System;

namespace CRM.Domain.Aggregates
{
    public class Account : BaseAggregateRoot<Account, Guid>
    {
        public Account()
        { }

        public Account(
            Guid id,
            string name,
            string website,
            string email,
            string phoneNumber,
            bool isActive,
            string userId
        ) : base(id)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentOutOfRangeException(nameof(name));

            Name = name;
            Website = website;
            Email = email;
            PhoneNumber = phoneNumber;
            IsActive = isActive;
            UserId = userId;

            this.AddEvent(new AccountCreated(this));
        }

        public string Name { get; private set; }
        public string Website { get; private set; }
        // TODO: Use value object instead of string?
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public bool IsActive { get; private set; }
        public string UserId { get; private set; }

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
                    UserId = account.UserId;
                    break;
            }
        }

        public static Account Create(
            string name,
            string website,
            string email,
            string phoneNumber,
            bool isActive,
            string userId
        )
        {
            return new Account(
                Guid.NewGuid(),
                name,
                website,
                email,
                phoneNumber,
                isActive,
                userId
            );
        }
    }
}
