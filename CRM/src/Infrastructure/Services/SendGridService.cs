using CRM.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Services
{
    public class SendGridService : ISendGridService
    {
        private readonly IConfiguration _configuration;
        private readonly SendGridClient _client;

        public SendGridService(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new SendGridClient(configuration.GetValue<string>("SendGridKey"));
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SendGridMessage msg = new SendGridMessage()
            {
                From = new EmailAddress(
                    _configuration.GetValue<string>("SendGridEmail"),
                    _configuration.GetValue<string>("SendGridUser")
                ),
                Subject = subject,
                HtmlContent = htmlMessage
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            await _client.SendEmailAsync(msg);
        }
    }
}
