namespace NoteMapper.Data.Core.Users
{
    public class RegistrationCode
    {
        public string Code { get; set; } = "";

        public DateTime CreatedUtc { get; set; }

        public DateTime? ExpiresUtc { get; set; }

        public string? Note { get; set; }

        public Guid RegistrationCodeId { get; set; }
    }
}
