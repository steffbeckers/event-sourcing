﻿using CRM.Application.Accounts.Commands.CreateAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
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
        public async Task<ActionResult<Guid>> Create([FromBody] CreateAccountDto dto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CreateAccountCommand command = new CreateAccountCommand(
                Guid.NewGuid(),
                dto.Name,
                dto.Website,
                dto.Email,
                dto.PhoneNumber,
                isActive: true
            );

            await Mediator.Publish(command, cancellationToken);

            return CreatedAtAction("GetById", new { id = command.Id }, command);
        }
    }
}
