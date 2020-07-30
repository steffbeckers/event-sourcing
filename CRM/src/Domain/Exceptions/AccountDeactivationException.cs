using CRM.Domain.Aggregates;
using System;

namespace CRM.Domain.Exceptions
{
    public class AccountDeactivationException : Exception
    {
        public Account Account { get; }

        public AccountDeactivationException(Account account, string message, Exception ex = null)
            : base(message ?? $"Account \"{account.Name}\" can't be deactivated.", ex)
        {
            this.Account = account;
        }
    }
}
