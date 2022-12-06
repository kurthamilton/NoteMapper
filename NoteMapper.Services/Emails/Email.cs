namespace NoteMapper.Services.Emails
{
    public class Email
    {
        public Email(string to, string subject, string body)
        {
            BodyHtml = body;
            Subject = subject;
            To = to;
        }

        public string BodyHtml { get; }

        public string Subject { get; }

        public string To { get; }
    }
}
