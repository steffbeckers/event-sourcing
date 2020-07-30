using CRM.Application.Common.Interfaces;
using CRM.Application.Common.Models;
using CRM.Domain.Entities;
using CRM.Domain.Events.Accounts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CRM.Application.Accounts.Events
{
    public class AccountEventsHandler : INotificationHandler<EventReceived<AccountCreated>>
    {
        private readonly IApplicationDbContext _context;

        public AccountEventsHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(EventReceived<AccountCreated> notification, CancellationToken cancellationToken)
        {
            // If this account already exists, we don't need to add it to the database
            Account existingAccount = await this._context.Accounts.FindAsync(notification.Event.AggregateId);
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

            await this._context.Accounts.AddAsync(newAccount);

            await this._context.SaveChangesAsync(cancellationToken);
        }
    }
}
