using CRM.Application.Common.Interfaces;
using CRM.Domain.Aggregates;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CRM.Application.Accounts.Commands.CreateAccount
{
    public partial class CreateAccountCommand : IRequest<Unit>
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

    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IEventsService<Account, Guid> _eventsService;

        public CreateAccountCommandHandler(ICurrentUserService currentUserService, IEventsService<Account, Guid> eventsService)
        {
            _currentUserService = currentUserService;
            _eventsService = eventsService;
        }

        public async Task<Unit> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            Account account = new Account(
                request.Id,
                request.Name,
                request.Website,
                request.Email,
                request.PhoneNumber,
                request.IsActive,
                this._currentUserService.UserId
            );

            await _eventsService.PersistAsync(account);

            return Unit.Value;
        }
    }
}
