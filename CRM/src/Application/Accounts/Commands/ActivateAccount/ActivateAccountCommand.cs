using CRM.Application.Common.Exceptions;
using CRM.Application.Common.Interfaces;
using CRM.Domain.Aggregates;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CRM.Application.Accounts.Commands.ActivateAccount
{
    public partial class ActivateAccountCommand : IRequest<Unit>
    {
        public ActivateAccountCommand(Guid accountId)
        {
            AccountId = accountId;
        }

        public Guid AccountId { get; private set; }
    }

    public class ActivateAccountCommandHandler : IRequestHandler<ActivateAccountCommand>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IEventsService<Account, Guid> _eventsService;

        public ActivateAccountCommandHandler(ICurrentUserService currentUserService, IEventsService<Account, Guid> eventsService)
        {
            _currentUserService = currentUserService;
            _eventsService = eventsService;
        }

        public async Task<Unit> Handle(ActivateAccountCommand request, CancellationToken cancellationToken)
        {
            Account account = await _eventsService.RehydrateAsync(request.AccountId);
            if (account == null)
                throw new NotFoundException(nameof(Account), request.AccountId);

            account.Activate(this._currentUserService.UserId);

            await _eventsService.PersistAsync(account);

            return Unit.Value;
        }
    }
}
