using System;

namespace CRM.Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
