using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using Mailjet.Client.TransactionalEmails.Response;
using NoteMapper.Core;
using NoteMapper.Services.Emails;

namespace NoteMapper.Emails
{
    public class MailjetEmailSenderService : IEmailSenderService
    {
        private readonly MailjetEmailSenderServiceSettings _settings;

        public MailjetEmailSenderService(MailjetEmailSenderServiceSettings settings)
        {
            _settings = settings;
        }

        public async Task<ServiceResult> SendEmailAsync(Email email)
        {
            MailjetClient client = new(_settings.ApiKey, _settings.ApiSecret);

            TransactionalEmail transactionalEmail = new TransactionalEmailBuilder()
                .WithFrom(new SendContact(_settings.FromEmail, _settings.FromName))
                .WithSubject(email.Subject)
                .WithHtmlPart(email.BodyHtml)
                .WithTo(new SendContact(email.To))
                .Build();

            TransactionalEmailResponse response = await client.SendTransactionalEmailAsync(transactionalEmail);
            if (IsSuccess(response))
            {
                return ServiceResult.Successful();
            }
            else
            {
                return ServiceResult.Failure("Error sending email");
            }
        }

        private static bool IsSuccess(TransactionalEmailResponse response)
        {
            return response.Messages
                .All(x => string.Equals(x.Status, "success", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}