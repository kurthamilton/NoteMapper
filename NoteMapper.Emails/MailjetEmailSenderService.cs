using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using Mailjet.Client.TransactionalEmails.Response;
using NoteMapper.Core;
using NoteMapper.Services.Emails;
using NoteMapper.Services.Logging;

namespace NoteMapper.Emails
{
    public class MailjetEmailSenderService : IEmailSenderService
    {
        private readonly IErrorLoggingService _errorLoggingService;
        private readonly MailjetEmailSenderServiceSettings _settings;

        public MailjetEmailSenderService(MailjetEmailSenderServiceSettings settings,
            IErrorLoggingService errorLoggingService)
        {
            _errorLoggingService = errorLoggingService;
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

            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "Mail.To", email.To },
                { "Mail.Subject", email.Subject },
                { "Mail.BodyHtml", email.BodyHtml }
            };

            for (int i = 0; i < response.Messages.Length; i++)
            {
                MessageResult messageResult = response.Messages[i];
                for (int j = 0; j < messageResult.Errors.Count; j++)
                {
                    data.Add($"Mail.Response.Messages[{i}].Errors[{j}]", messageResult.Errors[j].ErrorMessage);
                }
            }

            await _errorLoggingService.LogErrorMessageAsync("Error sending email", data);

            return ServiceResult.Failure("Error sending email");
        }

        private static bool IsSuccess(TransactionalEmailResponse response)
        {
            return response.Messages
                .All(x => string.Equals(x.Status, "success", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}