namespace NoteMapper.Data.Core.Users
{
    public class UserPassword
    {
        public string Hash { get; set; } = "";

        public string? ResetCode { get; set; }

        public DateTime? ResetCodeExpiresUtc { get; set; }

        public string Salt { get; set; } = "";

        public Guid UserId { get; set; }

        public Guid UserPasswordId { get; set; }

        public void RemoveResetCode()
        {
            ResetCode = null;
            ResetCodeExpiresUtc = null;
        }

        public void SetResetCode(string resetCode, DateTime expiresUtc)
        {
            ResetCode = resetCode;
            ResetCodeExpiresUtc = expiresUtc;
        }
    }
}
