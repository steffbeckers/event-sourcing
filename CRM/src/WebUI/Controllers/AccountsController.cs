using CRM.Application.Accounts.Commands.CreateAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CRM.WebUI.Controllers
{
    [Authorize]
    public class AccountsController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateAccountDTO dto)
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
