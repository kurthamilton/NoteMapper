using NoteMapper.Core;
using NoteMapper.Data.Core.Contact;
using NoteMapper.Services.Emails;
using NoteMapper.Services.Web.ViewModels.Contact;

namespace NoteMapper.Services.Web.Contact
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IEmailSenderService _emailSenderService;
        private readonly ContactServiceSettings _settings;

        public ContactService(IContactRepository contactRepository,
            IEmailSenderService emailSenderService, 
            ContactServiceSettings settings)
        {
            _contactRepository = contactRepository;
            _emailSenderService = emailSenderService;
            _settings = settings;
        }

        public async Task<ServiceResult> SendContactRequestAsync(ContactRequestViewModel request)
        {
            await _contactRepository.CreateAsync(new ContactRequest
            {
                CreatedUtc = DateTime.UtcNow,
                Email = request.Email,
                Message = request.Message
            });

            const string subject = "Note Mapper: New contact request";
            string body = $"<p>From: {request.Email}</p>" +
                          $"<p>{request.Message}</p>";

            Email email = new Email(_settings.ContactEmailAddress, subject, body);
            ServiceResult sendResult = await _emailSenderService.SendEmailAsync(email);

            return sendResult.Success
                ? ServiceResult.Successful("Your message has been sent")
                : ServiceResult.Failure("An error occurred while sending your request");
        }
    }
}
