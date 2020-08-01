using CRM.Domain.Common;
using CRM.Domain.Events;
using CRM.Domain.Events.Accounts;
using CRM.Domain.Exceptions;
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

        public void Activate(string userId)
        {
            if (this.IsActive)
            {
                throw new AccountActivationException(this, $"Account \"{this.Name}\" is already activated.");
            }

            this.AddEvent(new AccountActivated(this, userId));
        }

        public void Deactivate(string userId)
        {
            if (!this.IsActive)
            {
                throw new AccountDeactivationException(this, $"Account \"{this.Name}\" is already deactivated.");
            }

            this.AddEvent(new AccountDeactivated(this, userId));
        }

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
                case AccountActivated account:
                    IsActive = true;
                    break;
                case AccountDeactivated account:
                    IsActive = false;
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
