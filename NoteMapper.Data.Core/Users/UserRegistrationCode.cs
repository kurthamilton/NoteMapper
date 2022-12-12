namespace NoteMapper.Data.Core.Users
{
    public class UserRegistrationCode
    {
        public UserRegistrationCode(Guid userId, Guid registrationCodeId, 
            DateTime createdUtc) 
        { 
            CreatedUtc = createdUtc;
            RegistrationCodeId = registrationCodeId;
            UserId = userId;
        }

        public DateTime CreatedUtc { get; }

        public Guid RegistrationCodeId { get; }

        public Guid UserId { get; }
    }
}
