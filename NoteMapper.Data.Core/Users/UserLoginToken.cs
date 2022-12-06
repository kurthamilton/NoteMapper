namespace NoteMapper.Data.Core.Users
{
    public class UserLoginToken
    {
        public DateTime CreatedUtc { get; set; }

        public DateTime ExpiresUtc { get; set; }

        public string Token { get; set; } = "";

        public Guid UserLoginTokenId { get; set; }

        public Guid UserId { get; set; }
    }
}
