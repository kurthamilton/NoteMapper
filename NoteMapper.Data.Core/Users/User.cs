namespace NoteMapper.Data.Core.Users
{
    public class User
    {
        public DateTime CreatedUtc { get; set; }

        public string Email { get; set; } = "";

        public Guid UserId { get; set; }
    }
}
