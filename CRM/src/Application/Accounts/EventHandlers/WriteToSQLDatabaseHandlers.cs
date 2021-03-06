﻿using CRM.Application.Common.Interfaces;
using CRM.Application.Common.Models;
using CRM.Domain.Entities;
using CRM.Domain.Events.Accounts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CRM.Application.Accounts.EventHandlers
{
    public class WriteToSQLDatabaseHandlers : 
        INotificationHandler<EventReceived<AccountCreated>>,
        INotificationHandler<EventReceived<AccountActivated>>,
        INotificationHandler<EventReceived<AccountDeactivated>>
    {
        private readonly IApplicationDbContext _context;

        public WriteToSQLDatabaseHandlers(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(EventReceived<AccountCreated> notification, CancellationToken cancellationToken)
        {
            // If this account already exists, we don't need to add it to the database
            Account existingAccount = await _context.Accounts.FindAsync(notification.Event.AggregateId);
            if (existingAccount != null) { return; }

            // Create new account in SQL database
            Account newAccount = new Account()
            {
                Id = notification.Event.AggregateId,
                Name = notification.Event.Name,
                Website = notification.Event.Website,
                Email = notification.Event.Email,
                PhoneNumber = notification.Event.PhoneNumber,
                IsActive = notification.Event.IsActive,
                CreatedBy = notification.Event.UserId
            };

            await _context.Accounts.AddAsync(newAccount);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Handle(EventReceived<AccountActivated> notification, CancellationToken cancellationToken)
        {
            Account account = await this._context.Accounts.FindAsync(notification.Event.AggregateId);
            if (account == null) { return; }

            account.IsActive = true;
            account.LastModifiedBy = notification.Event.UserId;

            _context.Accounts.Update(account);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Handle(EventReceived<AccountDeactivated> notification, CancellationToken cancellationToken)
        {
            Account account = await this._context.Accounts.FindAsync(notification.Event.AggregateId);
            if (account == null) { return; }

            account.IsActive = false;
            account.LastModifiedBy = notification.Event.UserId;

            _context.Accounts.Update(account);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
