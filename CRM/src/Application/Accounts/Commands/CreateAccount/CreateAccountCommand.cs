using CRM.Application.Common.Interfaces;
using CRM.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CRM.Application.Accounts.Commands.CreateAccount
{
    public partial class CreateAccountCommand : INotification
    {
        public CreateAccountCommand(
            Guid id,
            string name,
            string website,
            string email,
            string phoneNumber,
            bool isActive
        )
        {
            Id = id;
            Name = name;
            Website = website;
            Email = email;
            PhoneNumber = phoneNumber;
            IsActive = isActive;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Website { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public bool IsActive { get; private set; }
    }

    public class CreateAccountCommandHandler : INotificationHandler<CreateAccountCommand>
    {
        private readonly IEventsService<Account, Guid> _eventsService;

        public CreateAccountCommandHandler(IEventsService<Account, Guid> eventsService)
        {
            _eventsService = eventsService;
        }

        public async Task Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            Account account = new Account(
                request.Id,
                request.Name,
                request.Website,
                request.Email,
                request.PhoneNumber,
                request.IsActive
            );

            await _eventsService.PersistAsync(account);
        }
    }
}
