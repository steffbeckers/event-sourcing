using CRM.Domain.Events.Accounts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Application.Common.Templates.Emails
{
    public partial class NewAccountCreatedEmailTemplate
    {
        private AccountCreated AccountCreatedEvent { get; set; }

        public NewAccountCreatedEmailTemplate(AccountCreated accountCreatedEvent)
        {
            AccountCreatedEvent = accountCreatedEvent;
        }
    }
}
