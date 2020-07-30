using CRM.Application.Common.Interfaces;
using CRM.Application.Common.Models;
using CRM.Application.Common.Templates.Emails;
using CRM.Domain.Entities;
using CRM.Domain.Events.Accounts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CRM.Application.Accounts.EventHandlers
{
    public class SendEmailHandlers : 
        INotificationHandler<EventReceived<AccountCreated>>,
        INotificationHandler<EventReceived<AccountDeactivated>>
    {
        private readonly ISendGridService _sendGridService;

        public SendEmailHandlers(ISendGridService sendGridService)
        {
            _sendGridService = sendGridService;
        }

        public async Task Handle(EventReceived<AccountCreated> notification, CancellationToken cancellationToken)
        {
            NewAccountCreatedEmailTemplate newAccountCreatedEmailTemplate = new NewAccountCreatedEmailTemplate(notification.Event);

            // TODO: This is a test email
            await _sendGridService.SendEmailAsync(
                email: "steff@steffbeckers.eu",
                subject: "ES CRM - New account created",
                htmlMessage: newAccountCreatedEmailTemplate.TransformText()
            );
        }

        public async Task Handle(EventReceived<AccountDeactivated> notification, CancellationToken cancellationToken)
        {
            // TODO: This is a test email
            await _sendGridService.SendEmailAsync(
                email: "steff@steffbeckers.eu",
                subject: "ES CRM - Account deactivated",
                htmlMessage: notification.Event.Name
            );
        }
    }
}
