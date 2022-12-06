namespace NoteMapper.Identity
{
    public interface IPasswordHasher
    {
        string EncodeSalt(byte[] salt);

        byte[] GenerateSalt();

        string HashPassword(string plainText, string salt);

        string HashPassword(string plainText, byte[] salt);
    }
}
