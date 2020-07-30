using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Identity
{
    public class EmailSender : IEmailSender
    {
        public EmailSenderAuthOptions options { get; } // set only via Secret Manager

        public EmailSender(IOptions<EmailSenderAuthOptions> optionsAccessor)
        {
            options = optionsAccessor.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SendGridClient client = new SendGridClient(options.SendGridKey);

            SendGridMessage msg = new SendGridMessage()
            {
                From = new EmailAddress(options.SendGridEmail, options.SendGridUser),
                Subject = subject,
                HtmlContent = htmlMessage
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            await client.SendEmailAsync(msg);
        }
    }

    public class EmailSenderAuthOptions
    {
        public string SendGridKey { get; set; }
        public string SendGridEmail { get; set; }
        public string SendGridUser { get; set; }
    }
}
