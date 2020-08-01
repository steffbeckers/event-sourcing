using CRM.Domain.Aggregates;
using System;

namespace CRM.Domain.Exceptions
{
    public class AccountActivationException : Exception
    {
        public Account Account { get; }

        public AccountActivationException(Account account, string message, Exception ex = null)
            : base(message ?? $"Account \"{account.Name}\" can't be activated.", ex)
        {
            this.Account = account;
        }
    }
}
