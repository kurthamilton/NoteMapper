namespace NoteMapper.Data.Core.Users
{
    public class RegistrationCode
    {
        public RegistrationCode(string code, DateTime? expiresUtc) 
        {
            Code = code;
            ExpiresUtc = expiresUtc;
        }   

        public string Code { get; }

        public DateTime? ExpiresUtc { get; }
    }
}
