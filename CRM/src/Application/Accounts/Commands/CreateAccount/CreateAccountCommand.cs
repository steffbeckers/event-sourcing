using CRM.Application.Common.Interfaces;
using CRM.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CRM.Application.Accounts.Commands.CreateAccount
{
    public partial class CreateAccountCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateAccountCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            Account account = new Account()
            {
                Name = request.Name,
                Website = request.Website,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            _context.Accounts.Add(account);

            await _context.SaveChangesAsync(cancellationToken);

            return account.Id;
        }
    }
}
