using AutoMapper;
using CRM.Application.Common.Mappings;

namespace CRM.Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountDTO
    {
        public string Name { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
