namespace NoteMapper.Data.Core.Contact
{
    public class ContactRequest
    {
        public DateTime CreatedUtc { get; set; }

        public string Email { get; set; } = "";

        public string Message { get; set; } = "";
    }
}
