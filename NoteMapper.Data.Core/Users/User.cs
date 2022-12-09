namespace NoteMapper.Data.Core.Users
{
    public class User
    {
        public User(Guid userId, DateTime createdUtc, string email)
        {
            CreatedUtc = createdUtc;
            Email = email;
            UserId = userId;
        }

        public DateTime CreatedUtc { get; }

        public string Email { get; }

        public Guid UserId { get; }
    }
}
