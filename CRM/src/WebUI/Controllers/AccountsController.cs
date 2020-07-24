using CRM.Application.Accounts.Commands.CreateAccount;
using GetAccounts = CRM.Application.Accounts.Queries.GetAccounts;
using GetAccountById = CRM.Application.Accounts.Queries.GetAccountById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.WebUI.Controllers
{
    [Authorize]
    public class AccountsController : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<GetAccounts.AccountDto>>> Get()
        {
            return await Mediator.Send(new GetAccounts.GetAccountsQuery());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetAccountById.AccountDto>> GetById(Guid id)
        {
            return await Mediator.Send(new GetAccountById.GetAccountByIdQuery() { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateAccountDto dto)
        {
            CreateAccountCommand command = new CreateAccountCommand() {
                Name = dto.Name,
                Website = dto.Website,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            return await Mediator.Send(command);
        }
    }
}
