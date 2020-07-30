using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Common.Interfaces
{
    public interface ISendGridService
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
