namespace NoteMapper.Data.Core.Users
{
    public class UserActivation
    {
        public UserActivation(Guid userId, DateTime createdUtc, DateTime expiresUtc, string code) 
        {
            Code = code;
            CreatedUtc = createdUtc;
            ExpiresUtc = expiresUtc;
            UserId = userId;
        }

        public string Code { get; }

        public DateTime CreatedUtc { get; }

        public DateTime ExpiresUtc { get; }

        public Guid UserId { get; }
    }
}
