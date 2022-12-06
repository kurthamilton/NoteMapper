namespace NoteMapper.Data.Core.Users
{
    public class UserActivation
    {
        public string Code { get; set; } = "";

        public DateTime CreatedUtc { get; set; }

        public DateTime ExpiresUtc { get; set; }

        public Guid UserActivationId { get; set; }

        public Guid UserId { get; set; }
    }
}
