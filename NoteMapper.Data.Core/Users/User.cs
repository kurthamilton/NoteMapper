namespace NoteMapper.Data.Core.Users
{
    public class User
    {
        public User(Guid userId, DateTime createdUtc, string email, DateTime? activatedUtc)
        {
            ActivatedUtc = activatedUtc;
            CreatedUtc = createdUtc;
            Email = email;
            UserId = userId;
        }

        public DateTime? ActivatedUtc { get; private set; }

        public DateTime CreatedUtc { get; }

        public string Email { get; }

        public Guid UserId { get; }

        public void Activate()
        {
            if (ActivatedUtc != null)
            {
                return;
            }

            ActivatedUtc = DateTime.UtcNow;
        }
    }
}
