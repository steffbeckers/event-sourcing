using AutoMapper;
using AutoMapper.QueryableExtensions;
using CRM.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CRM.Application.Accounts.Queries.GetAccountById
{
    public class GetAccountByIdQuery : IRequest<AccountDto>
    {
        public Guid Id { get; set; }
    }

    public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, AccountDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAccountByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AccountDto> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Accounts
                .Include(t => t.AccountContact)
                    .ThenInclude(t => t.Contact)
                .ProjectTo<AccountDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        }
    }
}
