using CRM.Application.Accounts.Commands.ActivateAccount;
using CRM.Application.Accounts.Commands.CreateAccount;
using CRM.Application.Accounts.Commands.DeactivateAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GetAccountById = CRM.Application.Accounts.Queries.GetAccountById;
using GetAccounts = CRM.Application.Accounts.Queries.GetAccounts;

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
        public async Task<IActionResult> Create([FromBody] CreateAccountDto dto, CancellationToken cancellationToken = default)
        {
            CreateAccountCommand command = new CreateAccountCommand(
                dto.Id,
                dto.Name,
                dto.Website,
                dto.Email,
                dto.PhoneNumber,
                isActive: true
            );

            await Mediator.Send(command, cancellationToken);

            return Ok();
        }

        [HttpPut("{id}/activate")]
        public async Task<IActionResult> Activate(Guid id, CancellationToken cancellationToken = default)
        {
            ActivateAccountCommand command = new ActivateAccountCommand(id);

            await Mediator.Send(command, cancellationToken);

            return Ok();
        }

        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken = default)
        {
            DeactivateAccountCommand command = new DeactivateAccountCommand(id);

            await Mediator.Send(command, cancellationToken);

            return Ok();
        }
    }
}
